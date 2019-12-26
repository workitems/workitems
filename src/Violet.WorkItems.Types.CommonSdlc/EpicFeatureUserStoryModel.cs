using System;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public static class EpicFeatureUserStoryModel
    {
        public static WorkItemDescriptor Epic
            => new WorkItemDescriptor("Epic", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
        public static WorkItemDescriptor Feature
            => new WorkItemDescriptor("Feature", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("Epic", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, new RelationshipValueProviderDescriptor("realize", "Epic")),
                    new PropertyDescriptor("Story", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("AcceptanceCriteria", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
        public static WorkItemDescriptor UserStory
            => new WorkItemDescriptor("UserStory", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("Feature", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, new RelationshipValueProviderDescriptor("realize", "Feature")),
                    new PropertyDescriptor("Story", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("AcceptanceCriteria", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, true, new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
    }
}
