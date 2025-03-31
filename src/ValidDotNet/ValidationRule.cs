namespace Frognar.ValidDotNet;

public sealed record ValidationRule<T>(Func<T, bool> IsInvalid, ValidationError Error) {
  public static implicit operator ValidationRule<T>((Func<T, bool> isInvalid, ValidationError error) rule)
    => Validation.Rule(rule.isInvalid).WithError(rule.error);
}

public static partial class Validation {
  readonly static ValidationError defaultError = Error("Validation failed");
  public static ValidationRule<T> Rule<T>(Func<T, bool> isInvalid) => new(isInvalid, defaultError);

  public static ValidationRule<T> WithError<T>(this ValidationRule<T> rule, ValidationError error) =>
    rule with { Error = error };
}