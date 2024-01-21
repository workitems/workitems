using System.Collections.Immutable;
using System.Threading.Tasks;
using Violet.WorkItems.Provider;
using Xunit;

namespace Violet.WorkItems.Query;

public class WiqlTest
{
    [Fact]
    public async Task WiqlHelper_SimpleTest()
    {
        // arrange
        var dataProvider = TestData.GetDataProvider();
        var query = new WorkItemsQuery(AndClause.Create(new ProjectClause("TestProj"), new WorkItemTypeClause("Bug"), new StringMatchClause("A", "A")));

        // act
        var actual = await dataProvider.ListWorkItemsAsync(query);

        // assert
        Assert.Collection(actual,
            w => Assert.Equal("T1", w.Id),
            w => Assert.Equal("T2", w.Id),
            w => Assert.Equal("T4", w.Id)
        );
    }

    [Fact]
    public async Task WiqlHelper_ValueMatchClause_Any()
    {
        // arrange
        var dataProvider = TestData.GetDataProvider();
        var query = new WorkItemsQuery(AndClause.Create(new ProjectClause("TestProj2"), new ValueMatchClause("A", [], true)));

        // act
        var actual = await dataProvider.ListWorkItemsAsync(query);

        // assert
        Assert.Collection(actual,
            w => Assert.Equal("T1", w.Id),
            w => Assert.Equal("T2", w.Id)
        );
    }
    [Fact]
    public async Task WiqlHelper_ValueMatchClause_OneOf()
    {
        // arrange
        var dataProvider = TestData.GetDataProvider();
        var query = new WorkItemsQuery(AndClause.Create(new ProjectClause("TestProj2"), new ValueMatchClause("A", ["A3", "B2"], false)));

        // act
        var actual = await dataProvider.ListWorkItemsAsync(query);

        // assert
        Assert.Collection(actual,
            w => Assert.Equal("T1", w.Id),
            w => Assert.Equal("T2", w.Id)
        );
    }
}



public static class TestData
{
    public static IDataProvider GetDataProvider()
        => new InMemoryDataProvider()
            .AddWorkItem("TestProj", "T1", "Bug", new Property("A", "String", "A1"))
            .AddWorkItem("TestProj", "T2", "Bug", new Property("A", "String", "A2"))
            .AddWorkItem("TestProj", "T3", "Bug", new Property("A", "String", "B1"))
            .AddWorkItem("TestProj", "T4", "Bug", new Property("A", "String", "A3"))
            .AddWorkItem("TestProj", "T5", "Bug", new Property("A", "String", "B2"))
            .AddWorkItem("TestProj", "T6", "Task", new Property("A", "String", "AX"))
            .AddWorkItem("TestProj2", "T1", "Bug", new Property("A", "String", "A3"))
            .AddWorkItem("TestProj2", "T2", "Bug", new Property("A", "String", "B2"))
            ;

    public static IDataProvider AddWorkItem(this IDataProvider self, string projectCode, string id, string type, params Property[] properties)
    {
        self.SaveNewWorkItemAsync(new WorkItem(projectCode, id, type, properties.ToImmutableArray(), ImmutableArray<LogEntry>.Empty)).Wait();

        return self;
    }

}