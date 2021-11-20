using Microsoft.AspNetCore.Components;
using Violet.WorkItems.Service.Messages;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.BlazorWebFrontend.Shared.Widgets;

public abstract class WidgetBase : ComponentBase
{
    [Parameter]
    public WorkItemDescriptorApiResponse WorkItemDescriptor { get; set; }
    [Parameter]
    public PropertyDescriptor PropertyDescriptor { get; set; }
    [Parameter]
    public WorkItem WorkItem { get; set; }
    [Parameter]
    public Property Property { get; set; }

    [Parameter]
    public EventCallback<WorkItem> WorkItemChanged { get; set; }

    public async Task OnChange(object value)
    {
        WorkItem = WorkItem.With(Property with
        {
            Value = value.ToString(),
        });

        await WorkItemChanged.InvokeAsync(WorkItem);
    }
}