using System;

namespace Violet.WorkItems.Types.CommonSdlc;

public static class EpicFeatureUserStoryModel
{
    public static WorkItemDescriptor Epic
        => WorkItemDescriptor.Create("Epic",
            new PropertyDescriptor[] {
                    PropertyDescriptor.Create("Title", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Open", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
            }
        );
    public static WorkItemDescriptor Feature
        => WorkItemDescriptor.Create("Feature",
            new PropertyDescriptor[] {
                    PropertyDescriptor.Create("Title", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("Epic", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: string.Empty, validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new RelationshipValueProviderDescriptor("realize", "Epic")),
                    // new PropertyDescriptor("Story", "String", validators:  new ValidatorDescriptor[] {
                    //     new MandatoryValidatorDescriptor(),
                    // }),
                    PropertyDescriptor.Create("AcceptanceCriteria", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Draft", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
            }
        );
    public static WorkItemDescriptor UserStory
        => WorkItemDescriptor.Create("UserStory",
            new PropertyDescriptor[] {
                    PropertyDescriptor.Create("Title", "String", validators:  new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("Feature", "String", propertyType: PropertyType.SingleValueFromProvider, validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new RelationshipValueProviderDescriptor("realize", "Feature")),
                    PropertyDescriptor.Create("Story", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("AcceptanceCriteria", "String", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }),
                    PropertyDescriptor.Create("State", "String", propertyType: PropertyType.SingleValueFromProvider, initialValue: "Draft", validators: new ValidatorDescriptor[] {
                        new MandatoryValidatorDescriptor(),
                    }, valueProvider: new EnumValueProviderDescriptor(new EnumValue("Draft", "Draft"), new EnumValue("Ready", "Ready"), new EnumValue("Open", "Open"), new EnumValue("Done", "Done"))),
            }
        );
}
