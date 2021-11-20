using Violet.WorkItems;
using Violet.WorkItems.Types;
namespace Violet.WorkItems.BlazorWebFrontend;

public class WorkItemApiResponseBase
{
    public bool Success { get; set; }
    public string ProjectCode { get; set; }
    public string WorkItemId { get; set; }
}
public class WorkItemBadRequestApiResponse : WorkItemApiResponseBase
{
    public IEnumerable<ErrorMessage> Errors { get; set; }
}

public class WorkItemDescriptorApiResponse : WorkItemApiResponseBase
{
    public IEnumerable<PropertyDescriptor> Properties { get; set; }
    public IEnumerable<CommandDescriptor> Commands { get; set; }
}

public class ValueProviderApiResponse : WorkItemApiResponseBase
{
    public string PropertyName { get; set; }
    public IEnumerable<ProvidedValue> Values { get; set; }
}

public class CreateWorkItemApiRequest
{
    public string ProjectCode { get; set; }
    public string WorkItemType { get; set; }
    public IEnumerable<Property> Properties { get; set; }
}
public class UpdateWorkItemApiRequest
{
    public string ProjectCode { get; set; }
    public string WorkItemId { get; set; }
    public string Comment { get; set; }
    public IEnumerable<Property> Properties { get; set; }
}

public class ExecuteCommandWorkItemApiRequest
{
    public string ProjectCode { get; set; }
    public string WorkItemId { get; set; }
    public string Command { get; set; }
}

public class WorkItemApiResponse : WorkItemApiResponseBase
{
    public WorkItem WorkItem { get; set; }
}

public class WorkItemListApiResponse
{
    public bool Success { get; set; }

    public IEnumerable<WorkItem> WorkItems { get; set; }
}


public class ProvidedValue
{
    public string Value { get; }
    public string DisplayText { get; }
    public bool IsAllowed { get; }

    public ProvidedValue(string value, string displayText, bool isAllowed = true)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        DisplayText = displayText ?? throw new ArgumentNullException(nameof(displayText));
        IsAllowed = isAllowed;
    }
}

public class ErrorMessage
{
    public ErrorMessage(string source, string errorCode, string message, string projectCode, string id, string property)
    {
        ProjectCode = projectCode;
        Id = id;
        Property = property;
        Source = source;
        ErrorCode = errorCode;
        Message = message;
    }

    public string ProjectCode { get; }
    public string Id { get; }
    public string Property { get; }
    public string Source { get; }
    public string ErrorCode { get; }
    public string Message { get; }
}
