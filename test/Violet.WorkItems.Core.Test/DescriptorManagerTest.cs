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
            var manager = new DescriptorManager(new InMemoryDescriptorProvider(new WorkItemDescriptor("Foo", new LogDescriptor(false, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("A", "String", PropertyType.SingleValue, true, true, Array.Empty<ValidatorDescriptor>(), null),
                new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, Array.Empty<ValidatorDescriptor>(), null),
            }, new StageDescriptor[] {
                new StageDescriptor("state-Triage", new PropertyValueConditionDescriptor("State", "Triage"), new StagePropertyDescriptor[] {
                    new StagePropertyDescriptor("A", null, null, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor()
                    })
                }, Array.Empty<CommandDescriptor>())
            })));

            await manager.LoadAllAsync();

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
                });
        }
    }
}