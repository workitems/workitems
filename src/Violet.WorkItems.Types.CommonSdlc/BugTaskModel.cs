using System;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public static class BugTaskModel
    {
        public static WorkItemDescriptor Bug
            => new WorkItemDescriptor("Bug", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                new PropertyDescriptor("Description", "String"),
                new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done")))
            }, Array.Empty<StageDescriptor>());

        public static WorkItemDescriptor Task
            => new WorkItemDescriptor("Task", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }),
                new PropertyDescriptor("Description", "String"),
                new PropertyDescriptor("State", "String", PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
            }, Array.Empty<StageDescriptor>());
    }
}