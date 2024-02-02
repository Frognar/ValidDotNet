namespace Frognar.ValidDotNet;

public static class Validation {
  public static ValidationError Error(string error) => new ValidationErrorMessage(error);
  public static ValidationError Error(string key, string error) => new ValidationErrorMessageWithKey(key, error);
}

public abstract record ValidationError;
public record ValidationErrorMessage(string Message) : ValidationError;
public record ValidationErrorMessageWithKey(string Key, string Message) : ValidationError;