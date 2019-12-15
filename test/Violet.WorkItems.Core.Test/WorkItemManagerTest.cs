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
            var result = await manager.Create("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(savedWorkItem);

            Assert.Equal("FOO", savedWorkItem.ProjectCode);
            Assert.Equal("BAR", savedWorkItem.WorkItemType);

            Assert.Collection(savedWorkItem.Properties,
                p =>
                {
                    Assert.Equal(p.Name, "A");
                    Assert.Equal(p.DataType, "String");
                    Assert.Equal(p.Value, "aa");
                },
                p =>
                {
                    Assert.Equal(p.Name, "B");
                    Assert.Equal(p.DataType, "String");
                    Assert.Equal(p.Value, "bb");
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
                await manager.Create(null, "FOO", new Property[] { });
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
                await manager.Create("FOO", null, new Property[] { });
            });
        }

        [Fact]
        public async Task WorkItemManager_Create_DoNotCreateWithoutProperties()
        {
            // arrange
            var providerMock = new Mock<IDataProvider>();
            var manager = new WorkItemManager(providerMock.Object, new CommonSdlcDescriptorProvider());

            // act
            var result = await manager.Create("FOO", "BAR", new Property[] { });

            // assert
            Assert.False(result.Success);
            providerMock.VerifyNoOtherCalls();
        }
    }
}
