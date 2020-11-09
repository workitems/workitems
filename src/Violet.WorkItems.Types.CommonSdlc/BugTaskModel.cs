using System;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public static class BugTaskModel
    {
        public static WorkItemDescriptor Bug
            => new WorkItemDescriptor("Bug", new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                new PropertyDescriptor("Description", "String"),
                new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", isEditable: false, validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done")))
            },
            log: new LogDescriptor(new LogEntryTypeDescriptor[] {
                new PropertyChangeLogEntryTypeDescriptor("bug-Done", "State", "✅", "Bug Closed by {user} at {date}", "Done")
            }),
            stages: new StageDescriptor[] {
                new StageDescriptor("stage-bug-open", new PropertyValueConditionDescriptor("State", "Open"), commands: new CommandDescriptor[] {
                    new ChangePropertyValueCommandDescriptor("command-close", "Close", "State", "Done"),
                })
            });

        public static WorkItemDescriptor Task
            => new WorkItemDescriptor("Task", new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                new PropertyDescriptor("Description", "String"),
                new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
            });
    }
}