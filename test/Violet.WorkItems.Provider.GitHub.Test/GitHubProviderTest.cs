using System;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Query;
using Xunit;

namespace Violet.WorkItems.Provider.GitHub;

public class GitHubProviderTest
{
    [Fact]
    public async Task GitHubProvider_AllWorkItemsAsync_PublicGitHubQuery()
    {
        // arrange
        var provider = new GitHubDataProvider("violet-workitems-github-unittest", null);

        // act
        var workItems = await provider.ListWorkItemsAsync(CommonQueries.OfProjectCode("workitems/workitems"));

        // assert
        Assert.NotEmpty(workItems);
        Assert.True(workItems.All(wi => wi.ProjectCode == "workitems/workitems"));
        Assert.True(workItems.All(wi => wi.WorkItemType == "Issue"));

        var workItem15 = workItems.FirstOrDefault(wi => wi.Id == "15");
        Assert.NotNull(workItem15);
        Assert.Equal("Version 1.0 Package Maintenance", workItem15[GitHubDataProvider.TitleProperty].Value);
        Assert.Equal("closed", workItem15[GitHubDataProvider.StateProperty].Value);
    }
}
