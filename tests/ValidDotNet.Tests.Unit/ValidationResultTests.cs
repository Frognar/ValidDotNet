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
  public void HasErrorsWhenErrorAdded() {
    ValidationResult result = Result().AddError(Error("error"));
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain(Error("error"));
  }

  [Fact]
  public void ThrowsExceptionWhenNullError() {
    ValidationError error = null!;
    Func<ValidationResult> act = () => Result().AddError(error);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void IsInvalidWhenErrorMessageAdded() {
    Result().AddError("error").IsValid.Should().BeFalse();
  }

  [Fact]
  public void HasErrorsWhenErrorMessageAdded() {
    ValidationResult result = Result().AddError("error");
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain(Error("error"));
  }

  [Fact]
  public void ThrowsExceptionWhenNullErrorMessage() {
    string error = null!;
    Func<ValidationResult> act = () => Result().AddError(error);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void IsInvalidWhenKeyedErrorAdded() {
    Result().AddError("Key", "error").IsValid.Should().BeFalse();
  }

  [Fact]
  public void HasErrorsWhenKeyedErrorAdded() {
    ValidationResult result = Result().AddError("key", "error");
    result.Errors.Should().HaveCount(1);
    result.Errors.Should().Contain(Error("key", "error"));
  }

  [Fact]
  public void ThrowsExceptionWhenNullKeyedErrorMessage() {
    string error = null!;
    Func<ValidationResult> act = () => Result().AddError("key", error);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ThrowsExceptionWhenNullKeyedErrorKey() {
    string key = null!;
    Func<ValidationResult> act = () => Result().AddError(key, "error");
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void HasMultipleErrorsWhenErrorAddedToInvalid() {
    ValidationResult result = ResultWith([Error("error1")]).AddError(Error("code1", "error2"));
    result.Errors.Should().HaveCount(2);
    result.Errors.Should().ContainInOrder(Error("error1"), Error("code1", "error2"));
  }

  [Theory]
  [MemberData(nameof(GetErrors))]
  public void AggregatesErrorsToSingleStringWithSeparator(ValidationError[] errors, string separator, string expected) {
    ResultWith(errors).AggregateErrors(separator).Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(GetMixedErrors))]
  public void AggregatesErrorsWithCodesToSingleStringWithSeperators(
    ValidationError[] errors,
    string separator,
    string keyValueSeparator,
    string expected) {
    ResultWith(errors).AggregateErrors(separator, keyValueSeparator).Should().Be(expected);
  }

  [Fact]
  public void ThrowsNotSupportedExceptionWhenDealingWithCustomError() {
    Func<string> act = () => ResultWith(Custom()).AggregateErrors();
    act.Should().Throw<NotSupportedException>();
  }

  [Theory]
  [MemberData(nameof(GetWithCustomErrors))]
  public void HandlesCustomErrorsWithSelectorMethod(
    ValidationError[] errors,
    Func<ValidationError, string> selector,
    string separator,
    string expected) {
    ResultWith(errors).AggregateErrors(selector, separator).Should().Be(expected);
  }

  public static IEnumerable<object[]> GetErrors() {
    yield return [Array.Empty<ValidationError>(), "\n", ""];
    yield return [new[] { Error("a") }, "\n", "a"];
    yield return [new[] { Error("a"), Error("b") }, "\n", "a\nb"];
    yield return [new[] { Error("a"), Error("b"), Error("c"), Error("d") }, ",", "a,b,c,d"];
  }

  public static IEnumerable<object[]> GetMixedErrors() {
    yield return [Array.Empty<ValidationError>(), ",", ":", ""];
    yield return [new[] { Error("c", "e") }, ",", ":", "c:e"];
    yield return [new[] { Error("c1", "e1"), Error("c2", "e1") }, ",", ":", "c1:e1,c2:e1"];
    yield return [new[] { Error("c1", "e1"), Error("c2", "e1"), Error("c2", "e2") }, "\n", "|", "c1|e1\nc2|e1\nc2|e2"];
    yield return [new[] { Error("c1", "e1"), Error("a"), Error("c2", "e2") }, "\n", "|", "c1|e1\na\nc2|e2"];
  }

  public static IEnumerable<object[]> GetWithCustomErrors() {
    yield return [Array.Empty<ValidationError>(), (ValidationError _) => "custom", ",", ""];
    yield return [new[] { Custom() }, (ValidationError _) => "custom", ",", "custom"];
    yield return [new[] { Custom(), Error("c1", "e1") }, OnlyCustom("custom"), ",", "custom,custom"];
    yield return [new[] { Custom(), Error("c1", "e1") }, KeyedWithCustom("custom", ":"), ",", "custom,c1:e1"];
    yield return [new[] { Custom(), Error("c1", "e1") }, KeyedWithCustom("custom", "|"), "\n", "custom\nc1|e1"];
    yield return [new[] { Custom(), Error("a"), Error("c1", "e1") }, AllTypes("custom", ":"), "\n", "custom\na\nc1:e1"];
  }
}