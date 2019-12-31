using System.Collections.Generic;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems
{
    public class WorkItemUpdatedResult
    {
        public bool Success { get; }
        public WorkItem? UpdatedWorkItem { get; }
        public IEnumerable<ErrorMessage> Errors { get; }

        public WorkItemUpdatedResult(bool success, WorkItem? changedWorkItem, IEnumerable<ErrorMessage> errors)
        {
            Success = success;
            UpdatedWorkItem = changedWorkItem;
            Errors = errors;
        }
    }
}