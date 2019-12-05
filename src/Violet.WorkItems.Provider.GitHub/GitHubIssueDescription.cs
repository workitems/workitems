using System;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Provider
{
    internal class GitHubIssueDescription
    {
        public WorkItemDescriptor Model
            => new WorkItemDescriptor("GitHubIssues",
                new LogDescriptor(false, new LogEntryTypeDescriptor[] {
                    new LogEntryTypeDescriptor("assignmentChange"),
                    new LogEntryTypeDescriptor("milestoneChange"),
                    new LogEntryTypeDescriptor("labelChange"),
                    new LogEntryTypeDescriptor("stateChange"),
                }),
                new PropertyDescriptor[] {
                    new PropertyDescriptor("State", "String", PropertyType.SingleValue, true, false, Array.Empty<ValidatorDescriptor>(), new ValueProviderDescriptor("Enum", "Open:Open,Closed:Closed")),
                    new PropertyDescriptor("Title", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new StringLengthValidatorDescriptor(3, 1000),
                        new MandatoryValidatorDescriptor(),
                    }, null),
                    new PropertyDescriptor("Description", "String", PropertyType.UserInput, true, true, new ValidatorDescriptor[] {
                        new StringLengthValidatorDescriptor(0, 4000),
                    }, null),
                    new PropertyDescriptor("Label", "String", PropertyType.MultipleValue, true, true, Array.Empty<ValidatorDescriptor>(), new ValueProviderDescriptor("ProjectCollectionValueProvider", "labels")),
                    new PropertyDescriptor("Milestone", "String", PropertyType.MultipleValue, true, true, Array.Empty<ValidatorDescriptor>(), new ValueProviderDescriptor("ProjectCollectionValueProvider", "milestones")),
                    new PropertyDescriptor("Assignee", "String", PropertyType.MultipleValue, true, true, Array.Empty<ValidatorDescriptor>(), new ValueProviderDescriptor("ProjectUserValueProvider", string.Empty)),
                },
                new StageDescriptor[] {
                    new StageDescriptor("stage-Open", new PropertyValueConditionDescriptor("State", "Open"),
                        Array.Empty<StagePropertyDescriptor>(),
                        new CommandDescriptor[] {
                            new ChangePropertyValueCommandDescriptor("Close", "Close", "State", "Closed"),
                        }
                    ),
                }
            );
    }
}