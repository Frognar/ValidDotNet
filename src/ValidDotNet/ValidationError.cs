namespace Frognar.ValidDotNet;

public record ValidationError(string Message);
public record ValidationErrorWithCode(string Code, string Message) : ValidationError(Message);