using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public class Validator<T>(params (Func<T, bool> isInvalid, string errorMessage)[] validators) {
  readonly ImmutableList<(Func<T, bool> isInvalid, string errorMessage)> validators = validators.ToImmutableList();

  public ValidationResult Validate(T item) {
    return validators.Any(v => v.isInvalid(item))
      ? new ValidationResult(["error"])
      : ValidationResult.valid;
  }
}