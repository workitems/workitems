using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.ValueProvider
{
    public class RelationshipValueProvider : IValueProvider
    {
        private readonly RelationshipValueProviderDescriptor _descriptor;
        private readonly WorkItemManager _workItemManager;
        private readonly string _projectCode;

        public RelationshipValueProvider(RelationshipValueProviderDescriptor descriptor, WorkItemManager workItemManager, string projectCode)
        {
            _descriptor = descriptor;
            _workItemManager = workItemManager;
            _projectCode = projectCode;
        }

        public Task<string> GetDefaultValueAsync()
            => Task.FromResult(string.Empty);

        public Task<IEnumerable<ProvidedValue>> ProvideAllValuesAsync()
            => SuggestionsAsync(string.Empty);

        public async Task<IEnumerable<ProvidedValue>> SuggestionsAsync(string input)
        {
            var collection = await _workItemManager.DataProvider.QueryWorkItemsAsync(new ListQuery(new AndClause(ImmutableArray.Create<BooleanClause>(
                new ProjectCodeEqualityClause(_projectCode),
                new WorkItemTypeEqualityClause(_descriptor.Type)
            )))) as ListQueryResult;

            return collection.WorkItems.Where(wi => wi.Id.StartsWith(input)).Select(wi => new ProvidedValue(EncodeRelationship(_descriptor.RelationshipType, _projectCode, wi.Id), wi.Id));
        }

        public async Task<bool> ValueExistsAsync(string value)
        {
            var result = false;

            var (_, projectCode, id) = DecodeRelationship(value);

            if (!(string.IsNullOrWhiteSpace(projectCode) || string.IsNullOrWhiteSpace(id)))
            {
                var workItem = await _workItemManager.GetAsync(projectCode, id);

                result = (workItem != null);
            }

            return result;
        }

        public bool IsValidEncoding(string value)
        {
            var (r, p, id) = DecodeRelationship(value);

            return !string.IsNullOrWhiteSpace(r) && !string.IsNullOrWhiteSpace(p) && !string.IsNullOrWhiteSpace(id);
        }

        public static string EncodeRelationship(string relationShipType, string projectCode, string id)
            => $"{relationShipType}:{projectCode}-{id}";
        public static (string relationshipType, string projectCode, string id) DecodeRelationship(string value)
        {
            var colonSeparator = value.IndexOf(":");
            var dashSeparator = value.IndexOf("-");

            if (colonSeparator != -1 && dashSeparator != -1)
            {

                var relationshipType = value.Substring(0, colonSeparator);
                var projectCode = value.Substring(colonSeparator + 1, dashSeparator - colonSeparator - 1);
                var id = value.Substring(dashSeparator + 1, value.Length - dashSeparator - 1);

                return (relationshipType, projectCode, id);
            }
            else
            {
                return (string.Empty, string.Empty, string.Empty);
            }
        }


        public bool IsUserExpierenceEnumerable => false;
    }
}