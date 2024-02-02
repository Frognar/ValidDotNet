using System.Collections.Immutable;

namespace Frognar.ValidDotNet;

/// <summary>
/// Represents the result of a validation operation, containing a collection of errors.
/// </summary>
/// <remarks>
/// Instances of this struct are immutable. Use the static 'valid' instance for successful validations.
/// </remarks>
public readonly record struct ValidationResult(ImmutableList<ValidationError> Errors) {
  public static ValidationResult valid = new();

  /// <summary>
  /// Gets a value indicating whether the validation result is valid, i.e., no errors are present.
  /// </summary>
  public bool IsValid { get; } = Errors.Count == 0;

  /// <summary>
  /// Gets the immutable list of errors associated with the validation result.
  /// </summary>
  public ImmutableList<ValidationError> Errors { get; } = Errors;

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationResult"/> struct with the specified errors.
  /// </summary>
  /// <param name="errors">The collection of errors.</param>
  public ValidationResult(IEnumerable<ValidationError> errors) : this(errors.ToImmutableList()) {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationResult"/> struct representing a valid result.
  /// </summary>
  public ValidationResult() : this(ImmutableList<ValidationError>.Empty) {
  }

  /// <summary>
  /// Adds a new error to the validation result.
  /// </summary>
  /// <param name="error">The error message to be added.</param>
  /// <returns>A new instance of <see cref="ValidationResult"/> with the added error.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided error is null.</exception>
  public ValidationResult AddError(ValidationError error) {
    ArgumentNullException.ThrowIfNull(error);
    return new ValidationResult(Errors.Append(error).ToImmutableList());
  }

  /// <summary>
  /// Aggregates the errors in the validation result into a single string.
  /// </summary>
  /// <param name="separator">The separator used to concatenate the error messages. Default is ",".</param>
  /// <param name="keyValueSeparator">The separator used between error code and message. Default is ":".</param>
  /// <returns>A concatenated string of errors with optional code and message separation.</returns>
  /// <exception cref="NotSupportedException">Thrown if containing custom error type.</exception>
  public string AggregateErrors(string separator = ",", string keyValueSeparator = ":")
    => AggregateErrors(DefaultErrorSelector(keyValueSeparator), separator);

  /// <summary>
  /// Aggregates the errors in the validation result into a single string using the specified selector and separator.
  /// </summary>
  /// <param name="selector">A function to extract a string representation from each ValidationError.</param>
  /// <param name="separator">The separator used to concatenate the error messages. Default is ",".</param>
  /// <returns>A concatenated string of errors based on the provided selector and separator.</returns>
  public string AggregateErrors(Func<ValidationError, string> selector, string separator = ",")
    => string.Join(separator, Errors.Select(selector));

  static Func<ValidationError, string> DefaultErrorSelector(string keyValueSeparator) => e => e switch
  {
    ValidationErrorMessageWithKey errorWithKey => $"{errorWithKey.Key}{keyValueSeparator}{errorWithKey.Message}",
    ValidationErrorMessage errorMessage => errorMessage.Message,
    _ => throw new NotSupportedException()
  };
}