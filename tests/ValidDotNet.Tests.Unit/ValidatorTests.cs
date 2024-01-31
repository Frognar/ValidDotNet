namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidatorTests {
  [Fact]
  public void CreateValidatorForInts() {
    Validator<int> validator = new();
  }
}