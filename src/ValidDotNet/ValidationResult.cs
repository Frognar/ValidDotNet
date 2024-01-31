using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

public readonly record struct ValidationResult {
  public static ValidationResult valid = new();

  public bool IsValid { get; }
  public ImmutableList<string> Errors { get; }

  public ValidationResult(IEnumerable<string> list) {
    Errors = list.ToImmutableList();
    IsValid = Errors.Count == 0;
  }

  public ValidationResult() {
    Errors = ImmutableList<string>.Empty;
    IsValid = true;
  }

  public ValidationResult AddError(string error) {
    ArgumentNullException.ThrowIfNull(error);
    return new ValidationResult(Errors.Append(error).ToList());
  }

  public string AggregateErrors(string separator) {
    return string.Join(separator, Errors);
  }
}