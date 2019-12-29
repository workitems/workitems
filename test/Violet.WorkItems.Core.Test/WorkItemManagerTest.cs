using System;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Xunit;
using Moq;
using Violet.WorkItems.Types.CommonSdlc;
using System.Collections.Generic;
using Violet.WorkItems.Provider.SqlServer;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Core.Test
{
    public class WorkItemManagerTest
    {
        private static IDataProvider GetSqlServerDataProvider()
        {
            var db = new SqlServerDataProvider($@"Server=localhost\SQLEXPRESS;Database=workitems_test;Trusted_Connection=True;");
            db.InitAsync().Wait();
            return db;
        }

        [Fact]
        public Task WorkItemManager_Create_SimpleWithoutDescriptor_OnInMemoryDataProvider()
            => WorkItemManager_Create_SimpleWithoutDescriptor(new InMemoryDataProvider());

        [Fact]
        [Trait("OnDataProvider", "SqlServer")]
        public Task WorkItemManager_Create_SimpleWithoutDescriptor_OnSqlServerDataProvider()
            => WorkItemManager_Create_SimpleWithoutDescriptor(GetSqlServerDataProvider());

        private async Task WorkItemManager_Create_SimpleWithoutDescriptor(IDataProvider dataProvider)
        {
            // arrange
            var manager = new WorkItemManager(dataProvider, new CommonSdlcDescriptorProvider());

            var properties = new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            };

            // act
            var result = await manager.CreateAsync("FOO", "BAR", properties);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.CreatedWorkItem);

            Assert.Equal("FOO", result.CreatedWorkItem.ProjectCode);
            Assert.Equal("BAR", result.CreatedWorkItem.WorkItemType);

            Assert.Collection(result.CreatedWorkItem.Properties,
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

        public Task WorkItemManager_Update_SimpleWithoutDescriptor_OnInMemoryDataProvider()
            => WorkItemManager_Update_SimpleWithoutDescriptor(new InMemoryDataProvider());

        [Fact]
        [Trait("OnDataProvider", "SqlServer")]
        public Task WorkItemManager_Update_SimpleWithoutDescriptor_OnSqlServerDataProvider()
            => WorkItemManager_Update_SimpleWithoutDescriptor(GetSqlServerDataProvider());

        private async Task WorkItemManager_Update_SimpleWithoutDescriptor(IDataProvider dataProvider)
        {
            // arrange
            var manager = new WorkItemManager(dataProvider, new CommonSdlcDescriptorProvider());

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

        [Fact]
        public Task WorkItemManager_Update_TwoTimesWithoutDescriptor_OnInMemoryDataProvider()
            => WorkItemManager_Update_TwoTimesWithoutDescriptor(new InMemoryDataProvider());

        [Fact]
        [Trait("OnDataProvider", "SqlServer")]
        public Task WorkItemManager_Update_TwoTimesWithoutDescriptor_OnSqlServerDataProvider()
            => WorkItemManager_Update_TwoTimesWithoutDescriptor(GetSqlServerDataProvider());

        private async Task WorkItemManager_Update_TwoTimesWithoutDescriptor(IDataProvider dataProvider)
        {
            // arrange
            var manager = new WorkItemManager(dataProvider, new CommonSdlcDescriptorProvider());

            var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

            // act
            var result1 = await manager.UpdateAsync("FOO", issue.Id, new Property[] {
                new Property("A", "String", "aab"),
            });

            var result2 = await manager.UpdateAsync("FOO", issue.Id, new Property[] {
                new Property("A", "String", "aabc"),
            });

            // assert
            Assert.NotNull(result1);
            Assert.True(result1.Success);
            Assert.NotNull(result1.UpdatedWorkItem);

            Assert.NotNull(result2);
            Assert.True(result2.Success);
            Assert.NotNull(result2.UpdatedWorkItem);

            Assert.Equal("FOO", result2.UpdatedWorkItem.ProjectCode);
            Assert.Equal("BAR", result2.UpdatedWorkItem.WorkItemType);

            Assert.Collection(result2.UpdatedWorkItem.Properties,
                p =>
                {
                    Assert.Equal("A", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("aabc", p.Value);
                },
                p =>
                {
                    Assert.Equal("B", p.Name);
                    Assert.Equal("String", p.DataType);
                    Assert.Equal("bb", p.Value);
                }
            );

            Assert.Collection(result2.UpdatedWorkItem.Log,
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
                },
                l =>
                {
                    Assert.Collection(l.Changes,
                        pc =>
                        {
                            Assert.Equal("A", pc.Name);
                            Assert.Equal("aab", pc.OldValue);
                            Assert.Equal("aabc", pc.NewValue);
                        }
                    );
                }
            );
        }

        [Fact]
        public async Task WorkItemManager_CreateTemplate_WithDefaultValues()
        {
            // arrange
            var workItemManager = new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
                new WorkItemDescriptor("BAR", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                    new PropertyDescriptor("A", "String", initialValue: "a"),
                    new PropertyDescriptor("B", "String"),
                }, Array.Empty<StageDescriptor>())
            ));

            // act
            var template = await workItemManager.CreateTemplateAsync("FOO", "BAR");

            // assert
            Assert.NotNull(template);
            Assert.Equal("BAR", template.WorkItemType);
            Assert.Collection(template.Properties,
                p =>
                {
                    Assert.Equal("A", p.Name);
                    Assert.Equal("a", p.Value);
                },
                p =>
                {
                    Assert.Equal("B", p.Name);
                    Assert.Equal("", p.Value);
                }
            );
        }
    }
}
