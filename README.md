# Which-Port

A C# port of the Unix `which` command for .NET 9 - an academic exercise in Test-Driven Development (TDD) and null-safe programming.

This code is probably over-engineered in a few weird ways it it was done to explore some new concepts and learn something new specifically:
- How to write null-safe code (by avoiding nulls)
- How to mock file systems in c#
- How to use the result and immutable patterns found in functional programming
- Starting with Tests and a lot of manual code, how we can iterate quickly with AI

The first few commits were done by hand in a few hours, and then I use AI to iterate over it.
I found that it generally stuck to the patterns and tests I started with, and that starting with tests made me confident in its changes.


## Overview

This implementation provides the core functionality of the Unix `which` command with a focus on:
- **Test-Driven Development**: Written using TDD principles
- **Null Safety**: Designed to avoid null reference exceptions through type-safe design
- **Clean Architecture**: Testable, maintainable code structure

**Note**: This is an academic exercise and has only been tested on macOS.

## Usage

```bash
# Build the project
dotnet build

# Find a single executable (first match)
dotnet run --project Which -- ls

# Find all matches in PATH
dotnet run --project Which -- -a grep

# Silent mode (no output, check exit code only)
dotnet run --project Which -- -s python

# Combine options
dotnet run --project Which -- -as node
```

## Command Line Options

- `-a`: Show all matches found in PATH (default: stop at first match)
- `-s`: Silent mode - no output, only exit codes
- Options can be combined: `-as` or `-sa`

## Exit Codes

- `0`: Success - executable found
- `1`: Failure - executable not found or error occurred

## Examples

```bash
# Basic usage
$ dotnet run --project Which -- python
/usr/bin/python

# Show all matches
$ dotnet run --project Which -- -a python
/usr/bin/python
/usr/local/bin/python

# Silent mode for scripting
$ dotnet run --project Which -- -s python
$ echo $?
0
```

## Architecture

The project follows clean architecture principles as an academic exercise:

- **Which**: Console application entry point
- **WhichLib**: Core business logic and services
- **WhichTests**: Comprehensive NUnit test suite

### TDD & Null Safety Features
- Result pattern for error handling without exceptions
- `NonNullableString` wrapper to prevent null string values
- Immutable value objects (readonly records/structs) for type safety
- Dependency injection with `System.IO.Abstractions` for testability
- Comprehensive test coverage driven by TDD approach

## Development

### Prerequisites

- .NET 9 SDK
- macOS (only tested platform)

### Building

```bash
dotnet build
```

### Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~CommandLineParserTests"
```

## Academic Notes

This project serves as an exploration of:
- Test-Driven Development methodology
- Null-safe programming patterns in C#
- Clean architecture principles
- Functional programming concepts (immutable data, Result types)

## License

See [LICENSE](LICENSE) file for details.