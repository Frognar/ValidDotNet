namespace Frognar.ValidDotNet;

/// <summary>
/// Static class providing methods for creating validation errors.
/// </summary>
public static class Validation {
  /// <summary>
  /// Creates a validation error with a simple error message.
  /// </summary>
  /// <param name="error">The error message.</param>
  /// <returns>An instance of <see cref="ValidationError"/> representing the error.</returns>
  public static ValidationError Error(string error) => new ValidationErrorMessage(error);
  
  /// <summary>
  /// Creates a validation error with a key and error message.
  /// </summary>
  /// <param name="key">The key associated with the error.</param>
  /// <param name="error">The error message.</param>
  /// <returns>An instance of <see cref="ValidationError"/> representing the error with a key.</returns>
  public static ValidationError Error(string key, string error) => new ValidationErrorMessageWithKey(key, error);
}

/// <summary>
/// Represents the base class for validation errors.
/// </summary>
public abstract record ValidationError;

/// <summary>
/// Represents a validation error with a message.
/// </summary>
public record ValidationErrorMessage(string Message) : ValidationError;

/// <summary>
/// Represents a validation error with a key and a message.
/// </summary>
public record ValidationErrorMessageWithKey(string Key, string Message) : ValidationError;