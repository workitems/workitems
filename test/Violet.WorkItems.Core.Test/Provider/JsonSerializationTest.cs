using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Violet.WorkItems.Provider;

public class JsonSerializationTest
{
    [Fact]
    public void JsonSerialization_Simple()
    {
        var workItem = new WorkItem("FOO", "FOO-1", "Bug", new Property[] {
            new Property("Title", "string", "Just a small test"),
            new Property("Description", "string", "Let us see whether serialization to JSON works")
        }.ToImmutableArray(),
        new LogEntry[] {
            new LogEntry(DateTimeOffset.Now, "ME", "Just a test", new PropertyChange[] {
                new PropertyChange("Title", "Foo", "Just a small test")
            }.ToImmutableArray())
        }.ToImmutableArray());

        var node = JsonSerializer.SerializeToNode(workItem);

        var actual = JsonSerializer.Deserialize<WorkItem>(node);

        Assert.Equal(workItem.ProjectCode, actual.ProjectCode);
        Assert.Equal(workItem.Id, actual.Id);
        Assert.Equal(workItem.WorkItemType, actual.WorkItemType);
        Assert.Collection(workItem.Properties, workItem.Properties.Select<Property, Action<Property>>(pExpected => ((Property pActual) => Assert.Equal(pExpected, pActual))).ToArray());
        Assert.Collection(workItem.Log, workItem.Log.Select<LogEntry, Action<LogEntry>>(pExpected => ((LogEntry pActual) => Assert.Equal(pExpected, pActual))).ToArray());
    }
}