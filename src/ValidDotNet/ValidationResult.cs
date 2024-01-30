namespace ValidDotNet;

public readonly record struct ValidationResult {
  public bool IsValid { get; }
  public ValidationResult(IReadOnlyCollection<string> list) {
    IsValid = false;
  }

  public ValidationResult() {
    IsValid = true;
  }
}