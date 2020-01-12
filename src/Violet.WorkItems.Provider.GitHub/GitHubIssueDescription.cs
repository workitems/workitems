using System;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Provider
{
    internal class GitHubIssueDescription
    {
        public WorkItemDescriptor Model
            => new WorkItemDescriptor("GitHubIssues",
                new PropertyDescriptor[] {
                    new PropertyDescriptor("State", "String", propertyType: PropertyType.SingleValueFromProvider, isEditable: false, initialValue: "Open", valueProvider: new EnumValueProviderDescriptor(new EnumValue("Open", "Open"), new EnumValue("Closed", "Closed"))),
                    new PropertyDescriptor("Title", "String", validators: new ValidatorDescriptor[] {
                        new StringLengthValidatorDescriptor(3, 1000),
                        new MandatoryValidatorDescriptor(),
                    }),
                    new PropertyDescriptor("Description", "String", validators: new ValidatorDescriptor[] {
                        new StringLengthValidatorDescriptor(0, 4000),
                    }),
                    new PropertyDescriptor("Label", "String", propertyType: PropertyType.MultipleValueFromProvider, valueProvider: new ProjectCollectionValueProviderDescriptor("labels")),
                    new PropertyDescriptor("Milestone", "String", propertyType: PropertyType.MultipleValueFromProvider, valueProvider: new ProjectCollectionValueProviderDescriptor("milestones")),
                    new PropertyDescriptor("Assignee", "String", propertyType: PropertyType.MultipleValueFromProvider, valueProvider: new ProjectUserValueProviderDescriptor(string.Empty)),
                },
                new StageDescriptor[] {
                    new StageDescriptor("stage-Open", new PropertyValueConditionDescriptor("State", "Open"),
                        Array.Empty<StagePropertyDescriptor>(),
                        new CommandDescriptor[] {
                            new ChangePropertyValueCommandDescriptor("Close", "Close", "State", "Closed"),
                        }
                    ),
                },
                new LogDescriptor(new LogEntryTypeDescriptor[] {
                    new PropertyChangeLogEntryTypeDescriptor("assignmentChange", "Assignee", "👩", "{user} assigned this to {new}"),
                    new PropertyChangeLogEntryTypeDescriptor("milestoneChange", "Milestone", "🏁", "{user} added this to the {new} milestone"),
                    new PropertyChangeLogEntryTypeDescriptor("labelChange", "Label", "🏷", "{user} added the {new} label"),
                    new PropertyChangeLogEntryTypeDescriptor("stateChange-Closed", "State", "🚫", "{user} closed this", "Closed"),
                })
            );
    }
}