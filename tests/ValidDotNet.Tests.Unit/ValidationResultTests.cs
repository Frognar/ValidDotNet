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

  record CustomError : ValidationError;

  [Fact]
  public void ThrowsNotSupportedExceptionWhenDealingWithCustomError() {
    Func<string> act = () => ResultWith(new CustomError()).AggregateErrors();
    act.Should().Throw<NotSupportedException>();
  }
}