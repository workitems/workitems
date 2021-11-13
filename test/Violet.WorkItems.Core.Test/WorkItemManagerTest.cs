using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Types.CommonSdlc;
using Xunit;

namespace Violet.WorkItems.Core.Test;

public class WorkItemManagerTest
{

    [Fact]
    public Task WorkItemManager_Create_SimpleWithoutDescriptor_OnInMemoryDataProvider()
        => WorkItemManager_Create_SimpleWithoutDescriptor(new InMemoryDataProvider());

    private static async Task WorkItemManager_Create_SimpleWithoutDescriptor(IDataProvider dataProvider)
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
        Assert.NotNull(result.ChangedWorkItem);

        Assert.Equal("FOO", result.ChangedWorkItem.ProjectCode);
        Assert.Equal("BAR", result.ChangedWorkItem.WorkItemType);

        Assert.Collection(result.ChangedWorkItem.Properties,
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
        await Assert.ThrowsAsync<ArgumentException>(async () => await manager.CreateAsync(null, "FOO", Array.Empty<Property>()));
    }

    [Fact]
    public async Task WorkItemManager_Create_MissingWorkItemType()
    {
        // arrange
        var manager = new WorkItemManager(null, new CommonSdlcDescriptorProvider());

        // act
        await Assert.ThrowsAsync<ArgumentException>(async () => await manager.CreateAsync("FOO", null, Array.Empty<Property>()));
    }

    [Fact]
    public async Task WorkItemManager_Create_DoNotCreateWithoutProperties()
    {
        // arrange
        var providerMock = new Mock<IDataProvider>();
        providerMock.SetupGet(o => o.Write).Returns(true);
        var manager = new WorkItemManager(providerMock.Object, new CommonSdlcDescriptorProvider());

        // act
        var result = await manager.CreateAsync("FOO", "BAR", Array.Empty<Property>());

        // assert
        Assert.False(result.Success);
        providerMock.VerifyGet(o => o.Write);
        providerMock.VerifyNoOtherCalls();
    }

    [Fact]

    public Task WorkItemManager_Update_SimpleWithoutDescriptor_OnInMemoryDataProvider()
        => WorkItemManager_Update_SimpleWithoutDescriptor(new InMemoryDataProvider());

    private static async Task WorkItemManager_Update_SimpleWithoutDescriptor(IDataProvider dataProvider)
    {
        // arrange
        var manager = new WorkItemManager(dataProvider, new CommonSdlcDescriptorProvider());

        var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

        // act
        var result = await manager.UpdateAsync("FOO", issue.ChangedWorkItem.Id, new Property[] {
                new Property("A", "String", "aab"),
            });

        // assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.ChangedWorkItem);

        Assert.Equal("FOO", result.ChangedWorkItem.ProjectCode);
        Assert.Equal("BAR", result.ChangedWorkItem.WorkItemType);

        Assert.Collection(result.ChangedWorkItem.Properties,
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

        Assert.Collection(result.ChangedWorkItem.Log,
            l => Assert.Collection(l.Changes,
                    pc =>
                    {
                        Assert.Equal("A", pc.Name);
                        Assert.Equal("aa", pc.OldValue);
                        Assert.Equal("aab", pc.NewValue);
                    }
                ));
    }

    [Fact]
    public Task WorkItemManager_Update_TwoTimesWithoutDescriptor_OnInMemoryDataProvider()
        => WorkItemManager_Update_TwoTimesWithoutDescriptor(new InMemoryDataProvider());


    private static async Task WorkItemManager_Update_TwoTimesWithoutDescriptor(IDataProvider dataProvider)
    {
        // arrange
        var manager = new WorkItemManager(dataProvider, new CommonSdlcDescriptorProvider());

        var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

        // act
        var result1 = await manager.UpdateAsync("FOO", issue.ChangedWorkItem.Id, new Property[] {
                new Property("A", "String", "aab"),
            });

        var result2 = await manager.UpdateAsync("FOO", issue.ChangedWorkItem.Id, new Property[] {
                new Property("A", "String", "aabc"),
            });

        // assert
        Assert.NotNull(result1);
        Assert.True(result1.Success);
        Assert.NotNull(result1.ChangedWorkItem);

        Assert.NotNull(result2);
        Assert.True(result2.Success);
        Assert.NotNull(result2.ChangedWorkItem);

        Assert.Equal("FOO", result2.ChangedWorkItem.ProjectCode);
        Assert.Equal("BAR", result2.ChangedWorkItem.WorkItemType);

        Assert.Collection(result2.ChangedWorkItem.Properties,
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

        Assert.Collection(result2.ChangedWorkItem.Log,
            l => Assert.Collection(l.Changes,
                    pc =>
                    {
                        Assert.Equal("A", pc.Name);
                        Assert.Equal("aa", pc.OldValue);
                        Assert.Equal("aab", pc.NewValue);
                    }
                ),
            l => Assert.Collection(l.Changes,
                    pc =>
                    {
                        Assert.Equal("A", pc.Name);
                        Assert.Equal("aab", pc.OldValue);
                        Assert.Equal("aabc", pc.NewValue);
                    }
                ));
    }

    [Fact]
    public async Task WorkItemManager_CreateTemplate_WithDefaultValues()
    {
        // arrange
        var workItemManager = new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
            WorkItemDescriptor.Create("BAR", new PropertyDescriptor[] {
                    PropertyDescriptor.Create("A", "String", initialValue: "a"),
                    PropertyDescriptor.Create("B", "String"),
            })
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

    [Fact]
    public async Task WorkItemManager_ExecuteCommand_NotFound()
    {
        // arrange
        var manager = new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
            WorkItemDescriptor.Create("BAR", new PropertyDescriptor[] {
                    PropertyDescriptor.Create("A", "String", initialValue: "a"),
                    PropertyDescriptor.Create("B", "String"),
            })
        ));

        var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

        // act
        var result = await manager.ExecuteCommandAsync("FOO", issue.ChangedWorkItem.Id, "Close");

        // assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task WorkItemManager_ExecuteCommand_Invoke()
    {
        // arrange
        var manager = new WorkItemManager(new InMemoryDataProvider(), new InMemoryDescriptorProvider(
            WorkItemDescriptor.Create("BAR", new PropertyDescriptor[] {
                    PropertyDescriptor.Create("A", "String", initialValue: "a"),
                    PropertyDescriptor.Create("B", "String"),
            },
            stages: new StageDescriptor[]{
                    new StageDescriptor("stage-aa", new PropertyValueConditionDescriptor("A", "aa"),
                        Array.Empty<StagePropertyDescriptor>(),
                        new CommandDescriptor[] {
                            new ChangePropertyValueCommandDescriptor("make-bb", "MakeBBC", "B", "bbc")
                        }),
            })
        ));

        var issue = await manager.CreateAsync("FOO", "BAR", new Property[] {
                new Property("A", "String", "aa"),
                new Property("B", "String", "bb"),
            });

        // act
        var result = await manager.ExecuteCommandAsync("FOO", issue.ChangedWorkItem.Id, "make-bb");

        // assert
        Assert.True(result.Success);
        Assert.Collection(result.ChangedWorkItem.Properties,
            p => { Assert.Equal("aa", p.Value); Assert.Equal("A", p.Name); },
            p => { Assert.Equal("bbc", p.Value); Assert.Equal("B", p.Name); }
        );
    }
}
