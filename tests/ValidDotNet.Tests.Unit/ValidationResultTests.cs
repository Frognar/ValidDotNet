namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidationResultTests {
  [Fact]
  public void IsValidWithoutErrors() {
    ValidationResult result = new();
    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void IsValidWhenEmptyErrors() {
    ValidationResult result = new([]);
    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void HasNoErrorsWhenNoErrorAdded() {
    ValidationResult result = new();
    result.Errors.Should().BeEmpty();
  }

  [Fact]
  public void IsInvalidWithErrors() {
    ValidationResult result = new(["error"]);
    result.IsValid.Should().BeFalse();
  }

  [Fact]
  public void HasErrorsWhenCreatedWithErrors() {
    ValidationResult result = new(["error"]);
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain("error");
  }

  [Fact]
  public void IsInvalidWhenErrorAdded() {
    ValidationResult result = new();
    result = result.AddError("error");
    result.IsValid.Should().BeFalse();
  }

  [Fact]
  public void ThrowsExceptionWhenNullError() {
    ValidationResult result = new();
    Func<ValidationResult> act = () => result.AddError(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void HasMultipleErrorsWhenErrorAddedToInvalid() {
    ValidationResult result = new(["error1"]);
    result = result.AddError("error2");
    result.Errors.Should().HaveCount(2);
    result.Errors.Should().ContainInOrder("error1", "error2");
  }

  [Theory]
  [InlineData(new string[] { }, "\n", "")]
  [InlineData(new[] { "a" }, "\n", "a")]
  [InlineData(new[] { "a", "b" }, "\n", "a\nb")]
  [InlineData(new[] { "a", "b", "c", "d" }, ",", "a,b,c,d")]
  public void AggregatesErrorsToSingleStringWithSeparator(string[] errors, string separator, string expected) {
    ValidationResult result = new(errors.ToList());
    string errorMessage = result.AggregateErrors(separator);
    errorMessage.Should().Be(expected);
  }
}