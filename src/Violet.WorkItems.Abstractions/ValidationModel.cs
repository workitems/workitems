namespace Violet.WorkItems.Validation;

public record ErrorMessage(string Source, string ErrorCode, string Message, string ProjectCode, string Id, string Property);