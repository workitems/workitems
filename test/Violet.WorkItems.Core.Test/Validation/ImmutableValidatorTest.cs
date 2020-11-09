using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Types.CommonSdlc;
using Xunit;

namespace Violet.WorkItems.Validation
{
    public class ImmutableValidatorTest
    {
        [Fact]
        public async Task ImmutableValidator_Validate_Success()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", string.Empty),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.CreatedWorkItem);
            Assert.Empty(result.Errors);
        }


        [Fact]
        public async Task ImmutableValidator_Validate_Error()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", ""),
                new Property("B", "String", "bb"),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.CreatedWorkItem);
            Assert.Collection(result.Errors,
                em =>
                {
                    Assert.Equal(nameof(ImmutableValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("B", em.Property);
                }
            );
        }

        [Fact]
        public async Task ImmutableValidator_Validate_UpdateError()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", string.Empty),
            };

            // act
            var result1 = await manager.CreateAsync("FOO", "BAR", properties);
            var result = await manager.UpdateAsync("FOO", result1.CreatedWorkItem.Id, new Property[] {
                new Property("B", "String", "bb")
            });

            // assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.NotNull(result.UpdatedWorkItem);
            Assert.Collection(result.Errors,
                em =>
                {
                    Assert.Equal(nameof(ImmutableValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("B", em.Property);
                }
            );
        }

        [Fact]
        public async Task ImmutableValidator_Validate_InternalEditSuccess()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", string.Empty),
            };

            // act
            var result1 = await manager.CreateAsync("FOO", "BAR", properties);
            var result = await manager.ExecuteCommandAsync("FOO", result1.CreatedWorkItem.Id, "set");

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.UpdatedWorkItem);
            Assert.Empty(result.Errors);
        }

        private static WorkItemManager BuildManager()
        {
            return new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
                new WorkItemDescriptor("BAR", new PropertyDescriptor[] {
                    new PropertyDescriptor("A", "String"),
                    new PropertyDescriptor("B", "String", isEditable: false),
                }, stages: new StageDescriptor[] {
                    new StageDescriptor("default", new PropertyValueConditionDescriptor("A", "aa"), commands: new CommandDescriptor[] {
                        new ChangePropertyValueCommandDescriptor("set", "SET B to a", "B", "a")
                    })
                })
            ));
        }
    }
}