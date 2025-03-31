namespace Frognar.ValidDotNet.Tests.Unit;

internal static class IntRules {
  public readonly static ValidationRule<int> MustBeOdd =
    Validation.Rule<int>(i => i % 2 == 0).WithError(Error("must be odd"));

  public readonly static ValidationRule<int> MustBeSmallerThan10 =
    Validation.Rule<int>(i => i >= 10).WithError(Error("must be smaller than 10"));
}