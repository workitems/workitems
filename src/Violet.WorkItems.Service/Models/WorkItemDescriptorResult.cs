using System.Collections.Generic;
using Violet.WorkItems.Types;
using Violet.WorkItems.Validation;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Service.Models
{
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
}