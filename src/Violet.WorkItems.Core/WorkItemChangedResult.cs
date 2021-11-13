using System.Collections.Generic;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public class WorkItemChangedResult
{
    public bool Success { get; }
    public WorkItem? ChangedWorkItem { get; }
    public IEnumerable<ErrorMessage> Errors { get; }

    public WorkItemChangedResult(bool success, WorkItem? changedWorkItem, IEnumerable<ErrorMessage> errors)
    {
        Success = success;
        ChangedWorkItem = changedWorkItem;
        Errors = errors;
    }
}
