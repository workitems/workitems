using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Xunit;

namespace Violet.WorkItems.Types;

public class DescriptorManagerTest
{
    [Fact]
    public async Task DescriptorManager_GetCurrentProperties_StageAddsValidator()
    {
        // arrange
        var manager = await LoadExampleDescriptorManagerAsync();

        var workItem = new WorkItem("BAR", "1234", "Foo", new Property[] {
                new Property("A", "String", ""),
                new Property("State", "String", "Triage"),
            }.ToImmutableArray(),
            ImmutableArray<LogEntry>.Empty);

        // act
        var properties = manager.GetCurrentPropertyDescriptors(workItem);

        // assert
        Assert.Collection(properties,
            pd =>
            {
                Assert.Equal("A", pd.Name);
                Assert.Collection(pd.Validators,
                    v => Assert.IsType<MandatoryValidatorDescriptor>(v));
            },
            pd =>
            {
                Assert.Equal("State", pd.Name);
                Assert.Empty(pd.Validators);
            });
    }


    [Fact]
    public async Task DescriptorManager_GetCurrentProperties_StageAddsNothing()
    {
        // arrange
        var manager = await LoadExampleDescriptorManagerAsync();

        var workItem = new WorkItem("BAR", "1234", "Foo", new Property[] {
                new Property("A", "String", ""),
                new Property("State", "String", "New"),
            }.ToImmutableArray(), ImmutableArray<LogEntry>.Empty);

        // act
        var properties = manager.GetCurrentPropertyDescriptors(workItem);

        // assert
        Assert.Collection(properties,
            pd =>
            {
                Assert.Equal("A", pd.Name);
                Assert.Empty(pd.Validators);
            },
            pd =>
            {
                Assert.Equal("State", pd.Name);
                Assert.Empty(pd.Validators);
            });
    }

    [Fact]
    public async Task DescriptorManager_GetCurrentCommands_StageAddsCommands()
    {
        // arrange
        var manager = await LoadExampleDescriptorManagerAsync();

        var workItem = new WorkItem("BAR", "1234", "Foo", new Property[] {
                new Property("A", "String", ""),
                new Property("State", "String", "Triage"),
            }.ToImmutableArray(), ImmutableArray<LogEntry>.Empty);

        // act
        var commands = manager.GetCurrentCommands(workItem);

        // assert
        Assert.Collection(commands,
            pd =>
            {
                Assert.Equal("command-close", pd.Name);
                Assert.Equal("Close", pd.Label);
            });
    }

    private static async Task<DescriptorManager> LoadExampleDescriptorManagerAsync()
    {
        var manager = new DescriptorManager(new InMemoryDescriptorProvider(WorkItemDescriptor.Create("Foo", "Foo", new PropertyDescriptor[] {
                PropertyDescriptor.Create("A", "String"),
                PropertyDescriptor.Create("State", "String"),
            }, new StageDescriptor[] {
                new StageDescriptor("state-Triage", new PropertyValueConditionDescriptor("State", "Triage"), new StagePropertyDescriptor[] {
                    new StagePropertyDescriptor("A", null, null, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor()
                    })
                }, new CommandDescriptor[] {
                    new ChangePropertyValueCommandDescriptor("command-close", "Close", "State", "Close")
                })
            })));

        await manager.LoadAllAsync();
        return manager;
    }
}
