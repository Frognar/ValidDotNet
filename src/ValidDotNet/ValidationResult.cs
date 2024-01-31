using System.Collections.ObjectModel;

namespace Frognar.ValidDotNet;

public readonly record struct ValidationResult {
  public bool IsValid { get; }
  public ReadOnlyCollection<string> Errors { get; }

  public ValidationResult(IReadOnlyCollection<string> list) {
    Errors = new ReadOnlyCollection<string>(list.ToList());
    IsValid = list.Count == 0;
  }

  public ValidationResult() {
    Errors = ReadOnlyCollection<string>.Empty;
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