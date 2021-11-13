using System.Collections.Generic;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public class WorkItemCommandExecutedResult
{
    public bool Success { get; }
    public WorkItem? UpdatedWorkItem { get; }
    public IEnumerable<ErrorMessage> Errors { get; }

    public WorkItemCommandExecutedResult(bool success, WorkItem? changedWorkItem, IEnumerable<ErrorMessage> errors)
    {
        Success = success;
        UpdatedWorkItem = changedWorkItem;
        Errors = errors;
    }
}
