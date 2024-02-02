namespace Frognar.ValidDotNet;

public abstract record ValidationError;
public record ValidationErrorMessage(string Message) : ValidationError;
public record ValidationErrorMessageWithKey(string Key, string Message) : ValidationError;