namespace ValidDotNet.Tests.Unit;

public class ValidationResultTests {
  [Fact]
  public void IsValidWithoutErrors() {
    ValidationResult result = new();
    result.IsValid.Should().Be(true);
  }

  [Fact]
  public void IsValidWhenEmptyErrors() {
    ValidationResult result = new([]);
    result.IsValid.Should().Be(true);
  }

  [Fact]
  public void IsInvalidWithErrors() {
    ValidationResult result = new(["error"]);
    result.IsValid.Should().Be(false);
  }

  [Fact]
  public void IsInvalidWhenErrorAdded() {
    ValidationResult result = new();
    result = result.AddError("error");
    result.IsValid.Should().Be(false);
  }
}