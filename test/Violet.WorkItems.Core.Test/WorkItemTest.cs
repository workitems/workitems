using System;
using System.Collections.Immutable;
using Xunit;

namespace Violet.WorkItems;

public class WorkItemTest
{
    [Fact]
    public void WorkItem_Indexer_Assignable()
    {
        var workItem = new WorkItem("A", "B", "Feature", new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "b"),
            }.ToImmutableArray(), ImmutableArray<LogEntry>.Empty);

        workItem = workItem.WithValue("A", "aa");

        Assert.Equal("aa", workItem["A"].Value);
        Assert.Collection(workItem.Properties,
            p => Assert.Equal("aa", p.Value),
            p => Assert.Equal("b", p.Value));
    }
}
