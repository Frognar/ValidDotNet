using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public class Validator<T>(ImmutableList<(Func<T, bool> isInvalid, string errorMessage)> rules) {

  public Validator(params (Func<T, bool> isInvalid, string errorMessage)[] rules)
    : this(rules.ToImmutableList()) {
  }

  public ValidationResult Validate(T item) {
    return rules
      .Where(v => v.isInvalid(item))
      .Select(v => v.errorMessage)
      .Aggregate(ValidationResult.valid, (result, error) => result.AddError(error));
  }

  public Validator<T> With(params (Func<T, bool> isInvalid, string errorMessage)[] extraRules) {
    return new Validator<T>(rules.Concat(extraRules).ToImmutableList());
  }
}