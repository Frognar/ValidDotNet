namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidatorTests {
  [Theory]
  [InlineData(1, true)]
  [InlineData(2, false)]
  [InlineData(int.MaxValue, true)]
  [InlineData(int.MinValue, false)]
  public void OddIntsAreConsideredValid(int value, bool expected) {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"));
    validator.Validate(value).IsValid.Should().Be(expected);
  }
}