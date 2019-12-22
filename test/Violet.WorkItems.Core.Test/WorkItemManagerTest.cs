using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Xunit;
using Moq;
using Violet.WorkItems.Types.CommonSdlc;

namespace Violet.WorkItems.Core.Test
{
    public class WorkItemManagerTest
    {
        [Fact]
        public async Task WorkItemManager_Create_SimpleWithoutDescriptor()
        {
            // arrange
            WorkItem savedWorkItem = null;
            var providerMock = new Mock<IDataProvider>();
            providerMock.Setup(p => p.SaveNewWorkItemAsync(It.IsAny<WorkItem>())).Callback<WorkItem>(wi => { savedWorkItem = wi; });
            var manager = new WorkItemManager(providerMock.Object, new CommonSdlcDescriptorProvider());

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(savedWorkItem);

            Assert.Equal("FOO", savedWorkItem.ProjectCode);
            Assert.Equal("BAR", savedWorkItem.WorkItemType);

            Assert.Collection(savedWorkItem.Properties,
                p =>
                {
                    Assert.Equal("A", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("aa", p.Value);
                },
                p =>
                {
                    Assert.Equal("B", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("bb", p.Value);
                }
            );
        }

        [Fact]
        public async Task WorkItemManager_Create_MissingProjectCode()
        {
            // arrange
            var manager = new WorkItemManager(null, new CommonSdlcDescriptorProvider());

            // act
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await manager.CreateAsync(null, "FOO", new Property[] { });
            });
        }

        [Fact]
        public async Task WorkItemManager_Create_MissingWorkItemType()
        {
            // arrange
            var manager = new WorkItemManager(null, new CommonSdlcDescriptorProvider());

            // act
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await manager.CreateAsync("FOO", null, new Property[] { });
            });
        }

        [Fact]
        public async Task WorkItemManager_Create_DoNotCreateWithoutProperties()
        {
            // arrange
            var providerMock = new Mock<IDataProvider>();
            var manager = new WorkItemManager(providerMock.Object, new CommonSdlcDescriptorProvider());

            // act
            var result = await manager.CreateAsync("FOO", "BAR", new Property[] { });

            // assert
            Assert.False(result.Success);
            providerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task WorkItemManager_Update_SimpleWithoutDescriptor()
        {
            // arrange
            var manager = new WorkItemManager(new InMemoryDataProvider(), new CommonSdlcDescriptorProvider());

            var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

            // act
            var result = await manager.UpdateAsync("FOO", issue.Id, new Property[] {
                new Property("A", "String", "aab"),
            });

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.UpdatedWorkItem);

            Assert.Equal("FOO", result.UpdatedWorkItem.ProjectCode);
            Assert.Equal("BAR", result.UpdatedWorkItem.WorkItemType);

            Assert.Collection(result.UpdatedWorkItem.Properties,
                p =>
                {
                    Assert.Equal("A", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("aab", p.Value);
                },
                p =>
                {
                    Assert.Equal("B", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("bb", p.Value);
                }
            );

            Assert.Collection(result.UpdatedWorkItem.Log,
                l =>
                {
                    Assert.Collection(l.Changes,
                        pc =>
                        {
                            Assert.Equal("A", pc.Name);
                            Assert.Equal("aa", pc.OldValue);
                            Assert.Equal("aab", pc.NewValue);
                        }
                    );
                }
            );
        }
    }
}
