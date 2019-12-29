using System;
using System.Threading.Tasks;
using Xunit;

namespace Violet.WorkItems.Types
{
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
            }, Array.Empty<LogEntry>());

            // act
            var properties = manager.GetCurrentPropertyDescriptors(workItem);

            // assert
            Assert.Collection(properties,
                pd =>
                {
                    Assert.Equal("A", pd.Name);
                    Assert.Collection(pd.Validators,
                        v =>
                        {
                            Assert.IsType<MandatoryValidatorDescriptor>(v);
                        });
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
            }, Array.Empty<LogEntry>());

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

        private static async Task<DescriptorManager> LoadExampleDescriptorManagerAsync()
        {
            var manager = new DescriptorManager(new InMemoryDescriptorProvider(new WorkItemDescriptor("Foo", new LogDescriptor(false, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("A", "String"),
                new PropertyDescriptor("State", "String"),
            }, new StageDescriptor[] {
                new StageDescriptor("state-Triage", new PropertyValueConditionDescriptor("State", "Triage"), new StagePropertyDescriptor[] {
                    new StagePropertyDescriptor("A", null, null, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor()
                    })
                }, Array.Empty<CommandDescriptor>())
            })));

            await manager.LoadAllAsync();
            return manager;
        }
    }
}