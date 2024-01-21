using Microsoft.AspNetCore.Components;
using Violet.WorkItems.Query;
using Violet.WorkItems.Service.Messages;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.BlazorWebFrontend.Shared.Wiql;

public class ValueSelectorBaseComponent<TClause> : WiqlBaseComponent
    where TClause : QueryClause
{
    [Parameter]
    public QueryClause Clause { get; set; } = default!;

    [Parameter]
    public EventCallback<QueryClause> ClauseChanged { get; set; }

    public TClause TypedClause
        => (TClause)Clause;

    public bool IsValidClause
        => Clause is TClause;

    protected async Task UpdateClauseAsync(QueryClause oldClause, QueryClause newClause)
    {
        Query = Query switch
        {
            null => new WorkItemsQuery(AndClause.Create(newClause)),
            { Clause: AndClause andClause } => new WorkItemsQuery(new AndClause(andClause.SubClauses.Replace(andClause.SubClauses.First(sc => sc.Id == oldClause.Id), newClause))),
            _ => Query,
        };

        await ReloadMetadataAsync();

        await QueryChanged.InvokeAsync(Query);

        if (oldClause.Id == Clause.Id)
        {
            Clause = newClause;

            await ClauseChanged.InvokeAsync(Clause);
        }
    }
}

public class WiqlBaseComponent : ComponentBase
{
    [Parameter]
    public WorkItemsQuery Query { get; set; } = default!;

    [Parameter]
    public EventCallback<WorkItemsQuery> QueryChanged { get; set; }

    [Parameter]
    public EventCallback<string> OnClose { get; set; }

    [Inject]
    public WorkItemDescriptorService WorkItemDescriptorService { get; set; } = default!;

    [Inject]
    public ValueProviderService ValueProviderService { get; set; } = default!;

    public string? ProjectCode { get; set; } = default;
    public string? WorkItemType { get; set; } = default;

    public IEnumerable<ProvidedValue>? ValuesForSelection { get; set; } = default;
    public IEnumerable<WorkItemType>? WorkItemTypes { get; set; } = default;
    public WorkItemDescriptorApiResponse? WorkItemDescriptor { get; set; } = null;


    protected override async Task OnParametersSetAsync()
    {
        await ReloadMetadataAsync();

        await base.OnParametersSetAsync();
    }

    protected async Task ReloadMetadataAsync()
    {
        ProjectCode = default;
        WorkItemType = default;

        if (Query is { Clause: AndClause andClause })
        {
            if (andClause.SubClauses.FirstOrDefault(sc => sc is ProjectClause) is ProjectClause { ProjectCode: not null } projectClause)
            {
                ProjectCode = projectClause.ProjectCode;
            }

            if (andClause.SubClauses.FirstOrDefault(sc => sc is WorkItemTypeClause) is WorkItemTypeClause { WorkItemType: not null } workItemTypeClause)
            {
                WorkItemType = workItemTypeClause.WorkItemType;
            }
        }

        if (ProjectCode is not null)
        {
            WorkItemTypes = (await WorkItemDescriptorService.GetWorkItemTypes(ProjectCode)).Types;
        }
        else
        {
            WorkItemTypes = null;
        }

        if (ProjectCode is not null && WorkItemType is not null)
        {
            WorkItemDescriptor = await WorkItemDescriptorService.GetWorkItemDescriptorByWorkItemType(ProjectCode, WorkItemType);
        }
        else
        {
            WorkItemDescriptor = null;
        }
    }

    protected async Task AddClauseAsync(QueryClause clause)
    {
        Query = Query switch
        {
            null => new WorkItemsQuery(AndClause.Create(clause)),
            { Clause: AndClause andClause } => new WorkItemsQuery(new AndClause(andClause.SubClauses.Add(clause))),
            { Clause: var anyClause } => new WorkItemsQuery(AndClause.Create(anyClause, clause)),
        };

        await ReloadMetadataAsync();

        await QueryChanged.InvokeAsync(Query);

        await OnClose.InvokeAsync("AddClause:" + clause.Id.ToString());
    }
    protected async Task RemoveClauseAsync(QueryClause clause)
    {
        Query = Query switch
        {
            null => new WorkItemsQuery(AndClause.Create(clause)),
            { Clause: AndClause andClause } => new WorkItemsQuery(new AndClause(andClause.SubClauses.RemoveAll(sc => sc.Id == clause.Id))),
            _ => Query,
        };

        await ReloadMetadataAsync();

        await QueryChanged.InvokeAsync(Query);

        await OnClose.InvokeAsync("RemoveClause");
    }

    protected async Task LoadValuesAsync(string propertyName)
    {
        if (ProjectCode is not null && WorkItemType is not null)
        {
            var providedValues = await ValueProviderService.GetValuesForTemplateProperty(ProjectCode, WorkItemType, propertyName);

            ValuesForSelection = providedValues;
        }
    }

}