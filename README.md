# ValidDotNet Library

The ValidDotNet library provides a lightweight validation framework for .NET applications, allowing you to define and apply validation rules to instances of specified types.

## Table of Contents

- [ValidDotNet Library](#validdotnet-library)
    - [Table of Contents](#table-of-contents)
    - [Overview](#overview)
    - [Installation](#installation)
    - [Usage](#usage)
        - [ValidationResult](#validationresult)
        - [Validator](#validator)
    - [Examples](#examples)
        - [Creating a ValidationResult](#creating-a-validationresult)

## Overview

The library consists of two main classes:

1. **ValidationResult**: Represents the result of a validation operation, containing a collection of errors. Instances are immutable, and a static 'valid' instance is provided for successful validations.

2. **Validator<T>**: A generic class for validating instances of a specified type using a set of rules. You can define custom validation rules and easily validate objects of type T.

## Installation

To use ValidDotNet in your project, you can install it via NuGet Package Manager:

```bash
dotnet add package Frognar.ValidDotNet
```

## Usage
### ValidationResult
The '**ValidationResult**' struct represents the outcome of a validation operation. It contains a collection of errors and provides methods to work with validation results.

### Validator<T>
The '**Validator<T>**' class allows you to define and apply validation rules to objects of type T. You can create a validator with an initial set of rules and then use it to validate objects.

## Examples
Here are some basic examples to get you started:

### Creating a ValidationResult
```cs
// Create a ValidationResult with no errors (valid result).
ValidationResult validResult = ValidationResult.valid;
ValidationResult otherValidResult = new();

// Create a validation result with errors.
ValidationResult invalidResult = new ValidationResult(new[]
{
    Validation.Error("Invalid input."), // new ValidationErrorMessage(Invalid input."),
    Validation.Error("Code123", "Invalid key."), // new ValidationErrorMessageWithKey("Code123", "Invalid key.")
    // Add more errors as needed
});

// Add a new error to the  existing validation result.
ValidationResult otherInvalidResult = validResult
    .AddError(new ValidationErrorMessage("Invalid input."))
    .AddError("Invalid input.") // Shorthand for adding a new error with a string message.
    .AddError(new ValidationErrorMessageWithKey("Code123", "Invalid key."))
    .AddError("Code123", "Invalid key."); // Shorthand for adding a new error with a key and a string message.

// Aggregate error messages into a single string.
string errors = invalidResult.AggregateErrors(separator: ", ", keyValueSeparator: "|");
Console.WriteLine($"Validation failed with errors: {errors}"); // "Validation failed with errors: Invalid input., Code123|Invalid key."

// Custom Error handling
ValidationResult invalidWithCustomError = validResult.AddError(new CustomError()).AddError("key", "error");
// string customError = invalidWithCustomError.AggregateErrors(); // Throws NotSupportedException
string customError = invalidWithCustomError.AggregateErrors(CustomSelector, separator: "\n");
Console.WriteLine($"Validation failed with errors: {customError}"); // "Validation failed with errors: custom\nkey:error 

record CustomError : ValidationError;
```

### Creating a Validator and Performing Validation
```cs
char[] invalidChars = ['a', 'b', 'c'];
List<(Func<CreateUserCommand, bool> isInvalid, ValidationError error)> rules =
[
  (u => string.IsNullOrWhiteSpace(u.Login), Validation.Error(nameof(CreateUserCommand.Login), "Login cannot be empty")),
  (u => u.Password.Length < 10, Validation.Error(nameof(CreateUserCommand.Password), "Password must be at least 10 characters long")),
  (u => u.Password.Any(c => invalidChars.Contains(c)), Validation.Error(nameof(CreateUserCommand.Password), "Password cannot contain 'a', 'b', or 'c'")),
];

Validator<CreateUserCommand> validator = new(rules);
ValidationResult validation = validator.Validate(new CreateUserCommand("user", "password"));
if (validation.IsValid) {
  // Handle success
} else {
  // Handle errors
  string errorMessages = validation.AggregateErrors(", ");
  Console.WriteLine($"Validation failed with errors: {errorMessages}"); // Validation failed with errors: Password:Password must be at least 10 characters long, Password:Password cannot contain 'a', 'b', or 'c'
}

record CreateUserCommand(string Login, string Password);
```
#### Inheritance supported!
```cs
public class OddIntsValidator() : Validator<int>([(i => i % 2 == 0, Validation.Error("Value must be odd"))]);
```

## License
This library is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.