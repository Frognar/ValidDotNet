namespace Frognar.ValidDotNet.Tests.Unit;

public static class Errors {
  public static ValidationError Error(string error) => new(error);
  public static ValidationError Error(string code, string error) => new ValidationErrorWithCode(code, error);
}