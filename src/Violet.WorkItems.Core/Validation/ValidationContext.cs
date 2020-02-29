using System.Collections.Generic;

namespace Violet.WorkItems.Validation
{
    public class ValidationContext
    {
        public ValidationContext(WorkItemManager workItemManager, WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            WorkItemManager = workItemManager;
            WorkItem = workItem;
            AppliedChanges = appliedChanges;
        }
        public WorkItemManager WorkItemManager { get; }
        public WorkItem WorkItem { get; }
        public IEnumerable<PropertyChange> AppliedChanges { get; }
    }
}