using System.Diagnostics;

namespace Frognar.ValidDotNet;

public class Validator<T> {
  public Validator((Func<T, bool> isInvalid, string errorMessage) validator) {
  }

  public ValidationResult Validate(T item) {
    return ValidationResult.valid;
  }
}