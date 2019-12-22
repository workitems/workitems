namespace Violet.WorkItems.Validation
{
    public class ErrorMessage
    {
        public ErrorMessage(string source, string errorCode, string message, string projectCode, string id, string property)
        {
            ProjectCode = projectCode;
            Id = id;
            Property = property;
            Source = source;
            ErrorCode = errorCode;
            Message = message;
        }

        public string ProjectCode { get; }
        public string Id { get; }
        public string Property { get; }
        public string Source { get; }
        public string ErrorCode { get; }
        public string Message { get; }
    }
}