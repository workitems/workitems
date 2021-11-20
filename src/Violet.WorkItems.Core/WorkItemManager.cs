using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public partial class WorkItemManager
{
    public IDataProvider DataProvider { get; set; }
    public DescriptorManager DescriptorManager { get; }
    public ValidationManager ValidationManager { get; }

    public static readonly string EmptyValue = string.Empty;

    public WorkItemManager(IDataProvider dataProvider, IDescriptorProvider descriptorProvider)
    {
        DataProvider = dataProvider;
        DescriptorManager = new DescriptorManager(descriptorProvider);
        ValidationManager = new ValidationManager(this, DescriptorManager);
    }

    public async Task<WorkItem> CreateTemplateAsync(string projectCode, string workItemType)
    {
        if (string.IsNullOrWhiteSpace(projectCode))
        {
            throw new ArgumentException("message", nameof(projectCode));
        }

        if (string.IsNullOrWhiteSpace(workItemType))
        {
            throw new ArgumentException("message", nameof(workItemType));
        }

        await InitAsync();


        var (success, propertyDescriptors) = DescriptorManager.GetAllPropertyDescriptors(workItemType);

        var properties = success
            ? propertyDescriptors.Select(pd => new Property(pd.Name, pd.DataType, pd.InitialValue ?? EmptyValue)).ToImmutableArray()
            : ImmutableArray<Property>.Empty;

        var wi = new WorkItem(projectCode, "NEW", workItemType, properties, ImmutableArray<LogEntry>.Empty);

        return wi;
    }

    public async Task<WorkItem?> GetAsync(string projectCode, string id)
    {
        if (string.IsNullOrWhiteSpace(projectCode))
        {
            throw new ArgumentException("message", nameof(projectCode));
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("message", nameof(id));
        }

        if (DataProvider is { Read: false })
        {
            throw new InvalidOperationException("DataProvider does not allow read operations");
        }

        await InitAsync();

        var workItem = await DataProvider.GetAsync(projectCode, id);

        return workItem;
    }

    public async Task<WorkItemChangedResult> CreateAsync(string projectCode, string workItemType, IEnumerable<Property> properties, bool autoCompleteFromTemplate = true)
        => CreateChangedResult(await CreateAsync(new Context(new CreateRequestFeature(projectCode, workItemType, properties.ToImmutableArray(), autoCompleteFromTemplate))));
    public async Task<Context> CreateAsync(Context context)
    {
        context = await InitMiddelwareAsync(context);
        context = await DetectChangesToTemplateMiddlewareAsync(context);
        context = await ApplyChangesAndWriteLogMiddlewareAsync(context);
        context = await ValidateMiddlewareAsync(context, false);
        context = await PersistMiddlewareAsync(context);

        return context;
    }

    public async Task<WorkItemChangedResult> UpdateAsync(string projectCode, string id, IEnumerable<Property> properties, string comment)
        => CreateChangedResult(await UpdateAsync(new Context(new UpdatePropertiesRequestFeature(projectCode, id, properties.ToImmutableArray(), comment))));
    public async Task<Context> UpdateAsync(Context context)
    {
        context = await InitMiddelwareAsync(context);
        context = await DetectChangesToExistingWorkItemMiddlewareAsync(context);
        context = await ApplyChangesAndWriteLogMiddlewareAsync(context);
        context = await ValidateMiddlewareAsync(context, false);
        context = await PersistMiddlewareAsync(context);

        return context;
    }

    public async Task<WorkItemChangedResult> ExecuteCommandAsync(string projectCode, string id, string command)
        => CreateChangedResult(await ExecuteCommandAsync(new Context(new ExecuteCommandRequestFeature(projectCode, id, command))));
    public async Task<Context> ExecuteCommandAsync(Context context)
    {
        context = await InitMiddelwareAsync(context);
        context = await ApplyCommandMiddlewareAsync(context);
        context = await ApplyChangesAndWriteLogMiddlewareAsync(context);
        context = await ValidateMiddlewareAsync(context, true);
        context = await PersistMiddlewareAsync(context);

        return context;
    }
}
