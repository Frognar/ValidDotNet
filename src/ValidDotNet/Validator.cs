namespace Frognar.ValidDotNet;

public class Validator<T> {
  readonly (Func<T, bool> isInvalid, string errorMessage) validator;

  public Validator((Func<T, bool> isInvalid, string errorMessage) validator) {
    this.validator = validator;
  }

  public ValidationResult Validate(T item) {
    return validator.isInvalid(item)
      ? new ValidationResult(["error"])
      : ValidationResult.valid;
  }
}