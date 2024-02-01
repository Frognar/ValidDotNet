namespace Frognar.ValidDotNet.Tests.Unit;

public class ValidationResultTests {
  static ValidationResult Result() => ValidationResult.valid;
  static ValidationResult ResultWith(params ValidationError[] errors) => new(errors);

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
    ResultWith(Error("error")).IsValid.Should().BeFalse();
  }

  [Fact]
  public void HasErrorsWhenCreatedWithErrors() {
    ValidationResult result = ResultWith([Error("error")]);
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain(Error("error"));
  }

  [Fact]
  public void IsInvalidWhenErrorAdded() {
    Result().AddError(Error("error")).IsValid.Should().BeFalse();
  }

  [Fact]
  public void ThrowsExceptionWhenNullError() {
    Func<ValidationResult> act = () => Result().AddError(null!);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void HasMultipleErrorsWhenErrorAddedToInvalid() {
    ValidationResult result = ResultWith([Error("error1")]).AddError(Error("error2"));
    result.Errors.Should().HaveCount(2);
    result.Errors.Should().ContainInOrder(Error("error1"), Error("error2"));
  }

  [Theory]
  [InlineData(new string[] { }, "\n", "")]
  [InlineData(new[] { "a" }, "\n", "a")]
  [InlineData(new[] { "a", "b" }, "\n", "a\nb")]
  [InlineData(new[] { "a", "b", "c", "d" }, ",", "a,b,c,d")]
  public void AggregatesErrorsToSingleStringWithSeparator(string[] errors, string separator, string expected) {
    ValidationError[] validationErrors = errors.Select(e => new ValidationError(e)).ToArray();
    ResultWith(validationErrors).AggregateErrors(separator).Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(GetErrors))]
  public void AggregatesErrorsWithCodesToSingleStringWithSeperators(
    ValidationError[] errors,
    string separator,
    string keyValueSeparator,
    string expected) {
    ResultWith(errors).AggregateErrors(separator, keyValueSeparator).Should().Be(expected);
  }

  public static IEnumerable<object[]> GetErrors() {
    yield return [new ValidationErrorWithCode[] { }, ",", ":", ""];
    yield return [new[] { new ValidationErrorWithCode("code1", "error1") }, ",", ":", "code1:error1"];
    yield return
    [
      new[]
      {
        new ValidationErrorWithCode("code1", "error1"),
        new ValidationErrorWithCode("code2", "error1")
      },
      ",", ":", "code1:error1,code2:error1"
    ];
    yield return
    [
      new[]
      {
        new ValidationErrorWithCode("code1", "error1"),
        new ValidationErrorWithCode("code2", "error1"),
        new ValidationErrorWithCode("code2", "error2")
      },
      "\n", "|", "code1|error1\ncode2|error1\ncode2|error2"
    ];
    yield return
    [
      new[]
      {
        new ValidationErrorWithCode("code1", "error1"),
        new ValidationError("superError"),
        new ValidationErrorWithCode("code2", "error2")
      },
      "\n", "|", "code1|error1\nsuperError\ncode2|error2"
    ];
  }
}