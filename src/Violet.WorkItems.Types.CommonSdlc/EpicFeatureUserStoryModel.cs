using System;

namespace Violet.WorkItems.Types.CommonSdlc
{
    public static class EpicFeatureUserStoryModel
    {
        public static WorkItemDescriptor Epic
            => new WorkItemDescriptor("Epic", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
        public static WorkItemDescriptor Feature
            => new WorkItemDescriptor("Feature", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("Epic", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: string.Empty, validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new RelationshipValueProviderDescriptor("realize", "Epic")),
                    new PropertyDescriptor("Story", "String", validators:  new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("AcceptanceCriteria", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Draft", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
        public static WorkItemDescriptor UserStory
            => new WorkItemDescriptor("UserStory", new LogDescriptor(true, Array.Empty<LogEntryTypeDescriptor>()),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("Title", "String", validators:  new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("Feature", "String", propertyType: PropertyType.SingleValueFromProvider, validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new RelationshipValueProviderDescriptor("realize", "Feature")),
                    new PropertyDescriptor("Story", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("AcceptanceCriteria", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Draft", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
                },
                Array.Empty<StageDescriptor>()
            );
    }
}
