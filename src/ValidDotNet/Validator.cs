namespace Frognar.ValidDotNet;

public class Validator<T> {
  readonly (Func<T, bool> isInvalid, string errorMessage) validator;

  public Validator(params (Func<T, bool> isInvalid, string errorMessage)[] validators) {
    validator = validators.First();
  }

  public ValidationResult Validate(T item) {
    return validator.isInvalid(item)
      ? new ValidationResult(["error"])
      : ValidationResult.valid;
  }
}