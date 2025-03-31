using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

/// <summary>
/// Generic class for validating instances of a specified type using a set of rules.
/// </summary>
/// <typeparam name="T">The type of object to be validated.</typeparam>
/// <param name="rules">The initial set of validation rules.</param>
public class Validator<T>(IEnumerable<ValidationRule<T>> rules) {
  /// <summary>
  /// Represents a collection of validation rules, each consisting of a condition and an associated error message.
  /// </summary>
  readonly ImmutableList<ValidationRule<T>> rules = rules.ToImmutableList();

  /// <summary>
  /// Validates the specified object against the set of rules and returns a <see cref="ValidationResult"/>.
  /// </summary>
  /// <param name="item">The object to be validated.</param>
  /// <returns>A <see cref="ValidationResult"/> indicating the validation outcome.</returns>
  public ValidationResult Validate(T item)
    => rules
      .Where(v => v.IsInvalid(item))
      .Select(v => v.Error)
      .Aggregate(ValidationResult.valid, (result, error) => result.AddError(error));

  /// <summary>
  /// Creates a new instance of <see cref="Validator{T}"/> by combining the current rules with additional rules.
  /// </summary>
  /// <param name="extraRules">Additional validation rules to be added.</param>
  /// <returns>A new instance of <see cref="Validator{T}"/> with the combined set of rules.</returns>
  public Validator<T> With(IEnumerable<ValidationRule<T>> extraRules)
    => new(rules.Concat(extraRules));
}