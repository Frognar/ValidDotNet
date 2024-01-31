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

  [Theory]
  [InlineData(1, true)]
  [InlineData(2, false)]
  [InlineData(-11, true)]
  [InlineData(11, false)]
  public void OddIntsSmallerThan10AreConsideredValid(int value, bool expected) {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"),
      (i => i > 10, "must be smaller than 10"));
    validator.Validate(value).IsValid.Should().Be(expected);
  }

  [Fact]
  public void AllIntsAreConsideredValidWhenNoRulesGiven() {
    Validator<int> validator = new();
    validator.Validate(1).IsValid.Should().BeTrue();
  }

  [Theory]
  [InlineData(1, "")]
  [InlineData(2, "must be odd")]
  [InlineData(11, "must be smaller than 10")]
  [InlineData(12, "must be odd, must be smaller than 10")]
  public void AllErrorsCollectedWhenMultipleRulesViolated(int value, string expected) {
    Validator<int> validator = new((i => i % 2 == 0, "must be odd"),
      (i => i > 10, "must be smaller than 10"));
    validator.Validate(value).AggregateErrors(", ").Should().Be(expected);
  }
}