# Which

A C# implementation of the Unix `which` command, created as an academic exercise to explore test-driven development with file system constraints and null-safe programming practices.

## Overview

This project implements a basic version of the Unix `which` utility that searches for executable files in the system PATH. It was developed as a learning exercise focusing on:

- **Test-Driven Development (TDD)** with file system operations
- **Null-safe programming** using custom wrapper types
- **Dependency injection** for testability
- **Functional programming patterns** with Result<T> for error handling

## Features

- Search for executable files in PATH
- List all matches with `-a` flag
- Basic command-line argument parsing
- Cross-platform .NET 9.0 implementation

## Limitations

This is an academic exercise and **missing several features** of the standard `which` command:

- **Silent mode (`-s`) is not implemented** - the flag is parsed but doesn't affect output
- Limited error handling compared to production `which`
- No support for additional `which` options like `-i` or `--version`
- **Only tested on Apple Mac** - may have platform-specific behaviors on other systems

## Architecture Highlights

### Null Safety
Uses `NonNullableString` wrapper type to minimize null reference possibilities:
```csharp
public struct NonNullableString
{
    // Ensures string values are never null
}
```

### Testable File System Operations
Leverages `System.IO.Abstractions` for dependency injection:
```csharp
public static void Search(ILogger logger, string pathToSearch, Settings settings, IFileSystem fileSystem)
```

### Functional Error Handling
Uses `Result<T>` pattern instead of exceptions:
```csharp
public static Result<Settings> OptionsToSettings(NonNullableString options, ImmutableArray<NonNullableString> files)
```

## Usage

```bash
dotnet run --project Which -- [options] [files...]
```

### Examples
```bash
# Find first match for 'ls'
dotnet run --project Which -- ls

# Find all matches for 'python'
dotnet run --project Which -- -a python

# Search for multiple files
dotnet run --project Which -- git make
```

## Building and Testing

```bash
# Build the project
dotnet build

# Run tests
dotnet test

# Run with specific arguments
dotnet run --project Which -- -a python
```

## Development Notes

This project was created as a learning exercise to understand:
- How to write testable code that interacts with the file system
- Patterns for avoiding null reference exceptions in C#
- Test-driven development practices with external dependencies
- Functional programming concepts in C#

The codebase prioritizes educational value and clean architecture over feature completeness.