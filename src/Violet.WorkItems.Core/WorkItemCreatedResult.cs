namespace Violet.WorkItems
{
    public class WorkItemCreatedResult
    {
        public bool Success { get; }
        public string Id { get; }

        public WorkItemCreatedResult(bool success, string id)
        {
            Success = success;
            Id = id;
        }
    }
}