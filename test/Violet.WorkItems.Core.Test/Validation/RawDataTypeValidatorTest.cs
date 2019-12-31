using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Xunit;

namespace Violet.WorkItems.Validation
{
    public class RawDataTypeValidatorTest
    {
        [Fact]
        public async Task RawDataTypeValidator_Validate_Success()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "Int32", "1234"),
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
        public async Task RawDataTypeValidator_Validate_SuccessWithNull()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "Int32", ""),
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
        public async Task MandatoryValidator_Validate_WrongData()
        {
            // arrange
            WorkItemManager manager = BuildManager();

            var properties = new Property[] {
                new Property("A", "String", ""),
                new Property("B", "Int32", "bb"),
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
                    Assert.Equal(nameof(RawDataTypeValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("B", em.Property);
                }
            );
        }

        [Fact]
        public async Task MandatoryValidator_Validate_DataTypeNotRecognized()
        {
            // arrange
            WorkItemManager manager = FaultyBuildManager();

            var properties = new Property[] {
                new Property("A", "String", ""),
                new Property("B", "X", "bb"),
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
                    Assert.Equal(nameof(RawDataTypeValidator), em.Source);
                    Assert.Equal(string.Empty, em.ErrorCode);
                    Assert.Equal("FOO", em.ProjectCode);
                    Assert.Equal("1", em.Id);
                    Assert.Equal("B", em.Property);
                }
            );
        }

        private static WorkItemManager BuildManager()
        {
            return new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
                new WorkItemDescriptor("BAR", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                    new PropertyDescriptor("A", "String"),
                    new PropertyDescriptor("B", "Int32"),
                }, Array.Empty<StageDescriptor>())
            ));
        }

        private static WorkItemManager FaultyBuildManager()
        {
            return new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
                new WorkItemDescriptor("BAR", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                    new PropertyDescriptor("A", "String"),
                    new PropertyDescriptor("B", "X"),
                }, Array.Empty<StageDescriptor>())
            ));
        }
    }
}