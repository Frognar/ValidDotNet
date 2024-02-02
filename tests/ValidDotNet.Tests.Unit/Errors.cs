namespace Frognar.ValidDotNet.Tests.Unit;

public static class Errors {
  record CustomError : ValidationError;

  public static ValidationError Error(string error) => new ValidationErrorMessage(error);
  public static ValidationError Error(string code, string error) => new ValidationErrorMessageWithKey(code, error);
  public static ValidationError Custom() => new CustomError();

  public static Func<ValidationError, string> OnlyCustom(string custom) => _ => custom;

  public static Func<ValidationError, string> KeyedWithCustom(string custom, string keyValueSeparator) => error =>
    error switch
    {
      ValidationErrorMessageWithKey withKey => ((Func<ValidationErrorMessageWithKey, string>)(error1 =>
        $"{error1.Key}{keyValueSeparator}{error1.Message}"))(withKey),
      _ => custom
    };

  public static Func<ValidationError, string> AllTypes(string custom, string keyValueSeparator) => error => error switch
  {
    ValidationErrorMessageWithKey withKey => $"{withKey.Key}{keyValueSeparator}{withKey.Message}",
    ValidationErrorMessage message => message.Message,
    _ => custom
  };
}