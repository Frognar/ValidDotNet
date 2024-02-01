using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public class Validator<T>(IEnumerable<(Func<T, bool> isInvalid, string errorMessage)> rules) {
  readonly ImmutableList<(Func<T, bool> isInvalid, string errorMessage)> rules = rules.ToImmutableList();

  public ValidationResult Validate(T item)
    => rules
      .Where(v => v.isInvalid(item))
      .Select(v => v.errorMessage)
      .Aggregate(ValidationResult.valid, (result, error) => result.AddError(error));

  public Validator<T> With(IEnumerable<(Func<T, bool> isInvalid, string errorMessage)> extraRules)
    => new(rules.Concat(extraRules));
}