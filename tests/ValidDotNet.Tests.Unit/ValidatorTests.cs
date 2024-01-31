namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidatorTests {
  [Fact]
  public void CreateValidatorForOddInts() {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"));
  }
}