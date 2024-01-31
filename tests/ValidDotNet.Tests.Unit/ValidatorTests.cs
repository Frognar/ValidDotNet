namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidatorTests {
  readonly static (Func<int, bool>, string) oddInts = (i => i % 2 == 0, "must be odd");
  readonly static (Func<int, bool>, string) smallerThan10 = (i => i >= 10, "must be smaller than 10");
  readonly Validator<int> oddIntsValidator = new(oddInts);
  readonly Validator<int> oddIntsSmallerThan10Validator = new(oddInts, smallerThan10);

  [Theory]
  [InlineData(1, true)]
  [InlineData(2, false)]
  [InlineData(int.MaxValue, true)]
  [InlineData(int.MinValue, false)]
  public void OddIntsAreConsideredValid(int value, bool expected) {
    oddIntsValidator.Validate(value).IsValid.Should().Be(expected);
  }

  [Theory]
  [InlineData(1, true)]
  [InlineData(2, false)]
  [InlineData(-11, true)]
  [InlineData(11, false)]
  public void OddIntsSmallerThan10AreConsideredValid(int value, bool expected) {
    oddIntsSmallerThan10Validator.Validate(value).IsValid.Should().Be(expected);
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void AllIntsAreConsideredValidWhenNoRulesGiven(int value) {
    new Validator<int>().Validate(value).IsValid.Should().BeTrue();
  }

  [Theory]
  [InlineData(1, "")]
  [InlineData(2, "must be odd")]
  [InlineData(11, "must be smaller than 10")]
  [InlineData(12, "must be odd, must be smaller than 10")]
  public void AllErrorsCollectedWhenMultipleRulesViolated(int value, string expected) {
    oddIntsSmallerThan10Validator.Validate(value).AggregateErrors(", ").Should().Be(expected);
  }

  [Theory]
  [InlineData(1, true)]
  [InlineData(2, false)]
  [InlineData(-11, true)]
  [InlineData(11, false)]
  public void CanAddMoreRulesToValidator(int value, bool expected) {
    oddIntsValidator.With(extraRules: [smallerThan10]).Validate(value).IsValid.Should().Be(expected);
  }
}