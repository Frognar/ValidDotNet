using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public class Validator<T>(params (Func<T, bool> isInvalid, string errorMessage)[] validators) {
  readonly ImmutableList<(Func<T, bool> isInvalid, string errorMessage)> validators = validators.ToImmutableList();

  public ValidationResult Validate(T item) {
    return validators
      .Where(v => v.isInvalid(item))
      .Select(v => v.errorMessage)
      .Aggregate(ValidationResult.valid, (result, error) => result.AddError(error));
  }
}