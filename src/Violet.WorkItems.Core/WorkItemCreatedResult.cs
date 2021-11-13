using System;
using System.Collections.Generic;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public class WorkItemCreatedResult
{
    public bool Success { get; }
    public WorkItem? CreatedWorkItem { get; }
    public IEnumerable<ErrorMessage> Errors { get; }

    public string Id => CreatedWorkItem?.Id ?? throw new InvalidOperationException($"No {nameof(CreatedWorkItem)} set.");

    public WorkItemCreatedResult(bool success, WorkItem? createdWorkItem, IEnumerable<ErrorMessage> errors)
    {
        Success = success;
        CreatedWorkItem = createdWorkItem;
        Errors = errors;
    }
}
