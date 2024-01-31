namespace Frognar.ValidDotNet;

public class Validator<T> {
  public Validator((Func<T, bool> isInvalid, string errorMessage) validator) {
  }
}