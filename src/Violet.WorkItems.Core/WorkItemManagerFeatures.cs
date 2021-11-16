using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Violet.WorkItems.Validation;

namespace Violet.WorkItems;

public static class ErrorFeatureExtensions
{
    public static Context AddError(this Context self, ErrorMessage error)
    {
        var errorFeature = self.Get<ErrorFeature>() ?? new ErrorFeature(ImmutableArray<ErrorMessage>.Empty);
        errorFeature = errorFeature with
        {
            Errors = errorFeature.Errors.Add(error),
        };
        self = self
            .With(errorFeature);
        return self;
    }
    public static Context AddErrors(this Context self, IEnumerable<ErrorMessage> errors)
    {
        var errorFeature = self.Get<ErrorFeature>() ?? new ErrorFeature(ImmutableArray<ErrorMessage>.Empty);
        errorFeature = errorFeature with
        {
            Errors = errorFeature.Errors.AddRange(errors),
        };
        self = self
            .With(errorFeature);
        return self;
    }
}

public record Context(ImmutableArray<object> Features)
{
    public Context(params object[] features)
        : this(features.ToImmutableArray())
    { }

    public TFeature? Get<TFeature>()
        => Features.OfType<TFeature>().FirstOrDefault();
    public Context With<TFeature>(TFeature feature)
        => this with
        {
            Features = Features.OfType<TFeature>().Any()
            ? Features.RemoveAll(f => f is TFeature).Add(feature)
            : Features.Add(feature),
        };
}

public record ErrorFeature(ImmutableArray<ErrorMessage> Errors);
public record WorkItemFeature(WorkItem WorkItem);
public record ChangesFeature(WorkItem OldWorkItem, ImmutableArray<PropertyChange> Changes, bool isNew);
public record CreateRequestFeature(string ProjectCode, string WorkItemType, ImmutableArray<Property> Properties, bool AutoCompleteFromTemplate);
public record UpdatePropertiesRequestFeature(string ProjectCode, string Id, ImmutableArray<Property> Properties);
public record ExecuteCommandRequestFeature(string ProjectCode, string Id, string Command);