namespace ValidDotNet;

public readonly record struct ValidationResult {
  public bool IsValid { get; }
  public ValidationResult(IReadOnlyCollection<string> list) {
    IsValid = list.Count == 0;
  }

  public ValidationResult() {
    IsValid = true;
  }

  public ValidationResult AddError(string error) {
    return new ValidationResult([error]);
  }
}