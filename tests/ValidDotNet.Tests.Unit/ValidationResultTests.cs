namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidationResultTests {
  static ValidationResult Result() => new();
  static ValidationResult ResultWith(params string[] errors) => new(errors);

  [Fact]
  public void IsValidWithoutErrors() {
    Result().IsValid.Should().BeTrue();
  }

  [Fact]
  public void IsValidWhenEmptyErrors() {
    ResultWith([]).IsValid.Should().BeTrue();
  }

  [Fact]
  public void HasNoErrorsWhenNoErrorAdded() {
    Result().Errors.Should().BeEmpty();
  }

  [Fact]
  public void IsInvalidWithErrors() {
    ResultWith("error").IsValid.Should().BeFalse();
  }

  [Fact]
  public void HasErrorsWhenCreatedWithErrors() {
    ValidationResult result = ResultWith(["error"]);
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain("error");
  }

  [Fact]
  public void IsInvalidWhenErrorAdded() {
    Result().AddError("error").IsValid.Should().BeFalse();
  }

  [Fact]
  public void ThrowsExceptionWhenNullError() {
    Func<ValidationResult> act = () => Result().AddError(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void HasMultipleErrorsWhenErrorAddedToInvalid() {
    ValidationResult result = ResultWith(["error1"]).AddError("error2");
    result.Errors.Should().HaveCount(2);
    result.Errors.Should().ContainInOrder("error1", "error2");
  }

  [Theory]
  [InlineData(new string[] { }, "\n", "")]
  [InlineData(new[] { "a" }, "\n", "a")]
  [InlineData(new[] { "a", "b" }, "\n", "a\nb")]
  [InlineData(new[] { "a", "b", "c", "d" }, ",", "a,b,c,d")]
  public void AggregatesErrorsToSingleStringWithSeparator(string[] errors, string separator, string expected) {
    ResultWith(errors).AggregateErrors(separator).Should().Be(expected);
  }
}