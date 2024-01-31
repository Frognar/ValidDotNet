using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public class Validator<T> {
  readonly ImmutableList<(Func<T, bool> isInvalid, string errorMessage)> validators;

  public Validator(params (Func<T, bool> isInvalid, string errorMessage)[] validators) {
    this.validators = validators.ToImmutableList();
  }

  public ValidationResult Validate(T item) {
    return validators.First().isInvalid(item)
      ? new ValidationResult(["error"])
      : ValidationResult.valid;
  }
}