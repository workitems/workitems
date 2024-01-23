using System.Text.Json;
using System.Text.Json.Serialization;

namespace Violet.WorkItems.Query;

public static class QuerySerialization
{
    public static JsonSerializerOptions GetOptions()
        => ConfigureJsonOptions(new JsonSerializerOptions());
    public static JsonSerializerOptions ConfigureJsonOptions(JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new BaseClassConverter<QueryClause>(
                    typeof(BooleanMultiClause),

                    typeof(ProjectClause),
                    typeof(WorkItemIdClause),
                    typeof(WorkItemTypeClause),
                    typeof(PropertyClause),
                    typeof(FullTextSearchClause),

                    typeof(AndClause),
                    typeof(OrClause),
                    typeof(NotClause),

                    typeof(StringMatchClause),
                    typeof(ValueMatchClause),
                    typeof(JoinClause)
                ));

        return options;
    }
}
