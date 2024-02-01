namespace Frognar.ValidDotNet.Tests.Unit;

public static class Errors {
  public static ValidationError Error(string error) => new(error);
}