namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidatorTests {
  [Fact]
  public void CreateValidatorForOddInts() {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"));
  }

  [Fact]
  public void OddIntsAreConsideredValid() {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"));
    validator.Validate(1).IsValid.Should().BeTrue();
  }
}