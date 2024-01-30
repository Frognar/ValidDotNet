namespace ValidDotNet.Tests.Unit;

public class ValidationResultTests {
  [Fact]
  public void CreateValidationResult() {
    _ = new ValidationResult();
  }

  [Fact]
  public void IsValidWithoutErrors() {
    ValidationResult result = new();
    result.IsValid.Should().Be(true);
  }
}