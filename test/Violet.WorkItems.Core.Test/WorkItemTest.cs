using Xunit;

namespace Violet.WorkItems
{
    public class WorkItemTest
    {
        [Fact]
        public void WorkItem_Indexer_Assignable()
        {
            var workItem = new WorkItem("A", "B", "Feature", new Property[] {
                new Property("A", "String", "a"),
                new Property("B", "String", "b"),
            }, new LogEntry[0]);

            workItem["A"].Value = "aa";

            Assert.Equal("aa", workItem["A"].Value);
            Assert.Collection(workItem.Properties,
                p => { Assert.Equal("aa", p.Value); },
                p => { Assert.Equal("b", p.Value); }
            );
        }
    }
}