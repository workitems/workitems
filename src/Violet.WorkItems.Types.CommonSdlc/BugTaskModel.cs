using System;

namespace Violet.WorkItems.Types.CommonSdlc;

public static class BugTaskModel
{
    public static WorkItemDescriptor Bug
        => WorkItemDescriptor.Create("Bug", new PropertyDescriptor[] {
                PropertyDescriptor.Create("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                PropertyDescriptor.Create("Description", "String"),
                PropertyDescriptor.Create("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", isEditable: false, validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done")))
        },
        log: new LogDescriptor(new LogEntryTypeDescriptor[] {
                new PropertyChangeLogEntryTypeDescriptor("bug-Done", "State", "✅", "Bug Closed by {user} at {date}", "Done")
        }),
        stages: new StageDescriptor[] {
                new StageDescriptor("stage-bug-open", new PropertyValueConditionDescriptor("State", "Open"),
                    Array.Empty<StagePropertyDescriptor>(),
                    new CommandDescriptor[] {
                        new ChangePropertyValueCommandDescriptor("command-close", "Close", "State", "Done"),
                    })
        });

    public static WorkItemDescriptor Task
        => WorkItemDescriptor.Create("Task", new PropertyDescriptor[] {
                PropertyDescriptor.Create("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                PropertyDescriptor.Create("Description", "String"),
                PropertyDescriptor.Create("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
        });
}
