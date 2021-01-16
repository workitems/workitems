using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Violet.WorkItems.Provider.GitHub
{
    public class GitHubProviderTest
    {
        [Fact]
        [Trait("OnDataProvider", "GitHub")]
        public async Task GitHubProvider_AllWorkItemsAsync_PublicGitHubQuery()
        {
            // arrange
            var provider = new GitHubDataProvider("violet-workitems-github-unittest", null);

            // act
            var workItems = await provider.QueryWorkItemsAsync(new ListQuery(new AndClause(ImmutableArray.Create<BooleanClause>(new ProjectCodeEqualityClause("workitems/workitems"))))) as ListQueryResult;

            // assert
            Assert.True(workItems.WorkItems.Count() > 0);
            Assert.True(workItems.WorkItems.All(wi => wi.ProjectCode == "workitems/workitems"));
            Assert.True(workItems.WorkItems.All(wi => wi.WorkItemType == "Issue"));

            var workItem15 = workItems.WorkItems.FirstOrDefault(wi => wi.Id == "15");
            Assert.NotNull(workItem15);
            Assert.Equal("Version 1.0 Package Maintenance", workItem15[GitHubDataProvider.TitleProperty].Value);
            Assert.Equal("closed", workItem15[GitHubDataProvider.StateProperty].Value);
        }
    }
}
