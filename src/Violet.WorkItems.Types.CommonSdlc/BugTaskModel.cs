using System;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public static class BugTaskModel
    {
        public static WorkItemDescriptor Bug
            => new WorkItemDescriptor("Bug", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, null),
                new PropertyDescriptor("Description", "String", PropertyType.UserInput, true, true, Array.Empty<ValidatorDescriptor>(), null),
                new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, new ValueProviderDescriptor("Enum", "Open:Open,Done:Done")),
            }, Array.Empty<StageDescriptor>());

        public static WorkItemDescriptor Task
            => new WorkItemDescriptor("Task", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()), new PropertyDescriptor[] {
                new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, null),
                new PropertyDescriptor("Description", "String", PropertyType.UserInput, true, true, Array.Empty<ValidatorDescriptor>(), null),
                new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                    new MandatoryValidatorDescriptor(),
                }, new ValueProviderDescriptor("Enum", "Open:Open,Done:Done")),
            }, Array.Empty<StageDescriptor>());
    }
}