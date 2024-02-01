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
dotnet add package ValidDotNet
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
ValidationResult newResult = new();
ValidationResult valid = ValidationResult.valid;
ValidationResult invalid = valid.AddError("error");
```

### Creating a Validator and Performing Validation
```cs
List<(Func<string, bool> isInvalid, string errorMessage)> rules = new() {
    (s => string.IsNullOrWhiteSpace(s), "String cannot be empty or whitespace."),
    (s => s.Length > 10, "String length should not exceed 10 characters.")
};

Validator<string> validator = new(rules);
ValidationResult validation = validator.Validate("SampleString");
if (validation.IsValid) {
    // Handle success
} else {
    // Handle errors
    string errorMessages = validation.AggregateErrors(", ");
    Console.WriteLine($"Validation failed with errors: {errorMessages}");
}
```
#### Inheritance supported!
```cs
public class OddIntsValidator() : Validator<int>([(i => i % 2 == 0, "Value must be odd")]);
```

## License
This library is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.