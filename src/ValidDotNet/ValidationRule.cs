namespace Frognar.ValidDotNet;

public sealed record ValidationRule<T>(Func<T, bool> IsInvalid, ValidationError Error) {
  public static implicit operator ValidationRule<T>((Func<T, bool> isInvalid, ValidationError error) rule)
    => new(rule.isInvalid, rule.error);
}