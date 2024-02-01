namespace Frognar.ValidDotNet;

public record ValidationError(string Message) {
  public static implicit operator ValidationError(string error) => new(error);
}