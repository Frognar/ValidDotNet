namespace ValidDotNet.Tests.Unit;

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
}