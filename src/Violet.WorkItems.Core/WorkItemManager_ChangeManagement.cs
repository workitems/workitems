using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public partial class WorkItemManager
{
    private async Task<Context> DetectChangesToTemplateMiddlewareAsync(Context context)
    {
        var createRequest = context.Get<CreateRequestFeature>() ?? throw new ArgumentException("feature not found", nameof(context));

        if (string.IsNullOrWhiteSpace(createRequest.ProjectCode))
        {
            throw new ArgumentException("message", nameof(createRequest.ProjectCode));
        }

        if (string.IsNullOrWhiteSpace(createRequest.WorkItemType))
        {
            throw new ArgumentException("message", nameof(createRequest.WorkItemType));
        }

        if (createRequest.Properties.Any())
        {
            var newIdentifer = (await DataProvider.NextNumberAsync(createRequest.ProjectCode)).ToString();

            // property changes for all values not identical with an empty template.
            var workItem = createRequest.AutoCompleteFromTemplate
                ? await CreateTemplateAsync(createRequest.ProjectCode, createRequest.WorkItemType)
                : new WorkItem(createRequest.ProjectCode, "NEW", createRequest.WorkItemType, ImmutableArray<Property>.Empty, ImmutableArray<LogEntry>.Empty);

            workItem = workItem with
            {
                Id = newIdentifer,
            };

            var propertyChanges = createRequest.Properties.Where(p => p.Value != EmptyValue).Select(p => new PropertyChange(p.Name, EmptyValue, p.Value));
            var newProperties = createRequest.Properties.AsEnumerable();

            propertyChanges = AnalyzeChanges(workItem.Properties, createRequest.Properties);
            newProperties = MergePropertySet(workItem.Properties, createRequest.Properties);

            createRequest = createRequest with
            {
                Properties = newProperties.ToImmutableArray(),
            };

            var changesFeature = new ChangesFeature(workItem, propertyChanges.ToImmutableArray(), true);

            context = context
                .With(changesFeature)
                .With(createRequest);
        }
        else
        {
            context = context.AddError(new ErrorMessage(nameof(WorkItemManager), string.Empty, "No properties provided.", createRequest.ProjectCode, string.Empty, string.Empty));
        }

        return context;
    }

    private async Task<Context> DetectChangesToExistingWorkItemMiddlewareAsync(Context context)
    {
        var updateRequest = context.Get<UpdatePropertiesRequestFeature>() ?? throw new ArgumentException($"no {nameof(UpdatePropertiesRequestFeature)} in context", nameof(context));

        if (string.IsNullOrWhiteSpace(updateRequest.ProjectCode))
        {
            throw new ArgumentException("message", nameof(updateRequest.ProjectCode));
        }

        if (string.IsNullOrWhiteSpace(updateRequest.Id))
        {
            throw new ArgumentException("message", nameof(updateRequest.Id));
        }

        var workItem = await GetAsync(updateRequest.ProjectCode, updateRequest.Id);

        if (workItem != null)
        {
            var changes = AnalyzeChanges(workItem.Properties, updateRequest.Properties);
            var newProperties = MergePropertySet(workItem.Properties, updateRequest.Properties);

            var changesFeature = new ChangesFeature(workItem, changes.ToImmutableArray(), false);
            updateRequest = updateRequest with
            {
                Properties = newProperties.ToImmutableArray(),
            };

            context = context
                .With(updateRequest)
                .With(changesFeature);
        }
        else
        {
            context = context.AddError(new ErrorMessage(nameof(WorkItemManager), string.Empty, $"The work item with id '{updateRequest.Id}' in project '{updateRequest.ProjectCode}' cannot be found.", updateRequest.ProjectCode, string.Empty, string.Empty));
        }

        return context;
    }

    private async Task<Context> ApplyChangesAndWriteLogMiddlewareAsync(Context context)
    {
        var changesFeature = context.Get<ChangesFeature>();
        var updateRequest = context.Get<UpdatePropertiesRequestFeature>();
        var createRequest = context.Get<CreateRequestFeature>();

        if (changesFeature is not null && (updateRequest is not null || createRequest is not null))
        {
            var changeProperties = updateRequest is not null
                ? updateRequest.Properties
                : createRequest.Properties;

            var properties = ApplyChangesToPropertySet(changeProperties, changesFeature.Changes);

            var newLog = updateRequest is not null
                ? changesFeature.OldWorkItem.Log.Union(new LogEntry[] { new LogEntry(DateTimeOffset.Now, "ABC", "Comment", changesFeature.Changes.ToImmutableArray()) }).ToImmutableArray()
                : ImmutableArray<LogEntry>.Empty;

            var newWorkItem = changesFeature.OldWorkItem with
            {
                Properties = properties.ToImmutableArray(),
                Log = newLog,
            };

            context = context.With(new WorkItemFeature(newWorkItem));
        }

        return context;
    }


    private static IEnumerable<PropertyChange> AnalyzeChanges(IEnumerable<Property> properties, IEnumerable<Property> requested)
        => requested
            .Select(r => (properties.FirstOrDefault(p => p.Name == r.Name), r))
            .Where(old_new => old_new.Item1?.Value != old_new.r.Value)
            .Select(old_new => old_new switch
                {
                    (null, var newProperty) => new PropertyChange(newProperty.Name, EmptyValue, newProperty.Value),
                    (var oldProperty, var newProperty) => new PropertyChange(oldProperty.Name, oldProperty.Value, newProperty.Value),
                });

    private static IEnumerable<Property> ApplyChangesToPropertySet(IEnumerable<Property> properties, IEnumerable<PropertyChange> changes)
        => properties.Select(p => changes.FirstOrDefault(c => c.Name == p.Name) switch
        {
            PropertyChange change => new Property(p.Name, p.DataType, change.NewValue),
            null => p,
        });

    private static IEnumerable<Property> MergePropertySet(IEnumerable<Property> properties, IEnumerable<Property> requested)
        => properties.Union(requested.Where(pr => !properties.Any(p => pr.Name == p.Name)));

}