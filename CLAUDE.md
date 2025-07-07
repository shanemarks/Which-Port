# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a C# implementation of the Unix `which` command, targeting .NET 9.0. The project implements a command-line utility that searches for executable files in the system PATH.

## Project Structure

- **Which/** - Main console application entry point
- **WhichLib/** - Core library containing business logic
  - **Data/** - Data structures (Settings, Result<T>, NonNullableString)
  - **Services/** - Core services (SearchService, CommandLineProcessor, PathOperations)
  - **interfaces/** - Interface definitions (ILogger)
- **WhichTests/** - NUnit test project

## Common Commands

### Building
```bash
dotnet build
dotnet build -c Release
```

### Running Tests
```bash
dotnet test
dotnet test --verbosity normal
```

### Running the Application
```bash
dotnet run --project Which
dotnet run --project Which -- [options] [files...]
```

### Publishing
```bash
dotnet publish -c Release
```

## Architecture Notes

### Core Flow
1. **Program.cs** - Entry point that parses command line arguments
2. **CommandLineProcessor** - Validates options and converts to Settings
3. **SearchService** - Orchestrates the search using PathOperations
4. **PathOperations** - Handles PATH parsing and file system operations

### Key Design Patterns
- **Result<T>** pattern for error handling instead of exceptions
- **NonNullableString** wrapper for string safety
- **IFileSystem** abstraction for testability using System.IO.Abstractions
- **ILogger** interface for console output abstraction

### Command Line Options
- `-a` - List all matches (not just first)
- `-s` - Silent mode
- Options can be combined: `-as` or `-sa`

### Dependencies
- **System.IO.Abstractions** - File system abstraction for testing
- **NUnit** - Testing framework
- **coverlet.collector** - Code coverage

## Testing Strategy

Tests use System.IO.Abstractions.TestingHelpers for mocking file system operations. Test files follow the pattern `{ClassName}Tests.cs` and cover:
- Command line processing validation
- Path operations with mocked file system
- Search service behavior with various scenarios
- Data structure validation