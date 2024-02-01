using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public readonly record struct ValidationResult(ImmutableList<string> Errors) {
  public static ValidationResult valid = new();

  public bool IsValid { get; } = Errors.Count == 0;
  public ImmutableList<string> Errors { get; } = Errors;

  public ValidationResult(IEnumerable<string> errors) : this(errors.ToImmutableList()) {
  }

  public ValidationResult() : this(ImmutableList<string>.Empty) {
  }

  public ValidationResult AddError(string error) {
    ArgumentNullException.ThrowIfNull(error);
    return new ValidationResult(Errors.Append(error));
  }

  public string AggregateErrors(string separator)
    => string.Join(separator, Errors);
}