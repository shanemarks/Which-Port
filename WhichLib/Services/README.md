# Services Layer

This folder contains business logic services that orchestrate the core functionality of the Which application.

## Files

### Command Line Processing
- **CommandLineParser.cs**: Parses raw command line arguments into structured `ParsedArguments`
  - Separates command options (like "-a", "-s") from file names to search
  - Returns `Result<ParsedArguments>` for functional error handling

- **CommandLineProcessor.cs**: Validates parsed options and converts them to application settings
  - Validates command line options (only 'a' and 's' are valid)
  - Maps options to `OutputMode` and `SearchMode` enums
  - Returns `Result<Settings>` with detailed error messages for invalid options

### Core Search Logic
- **SearchService.cs**: Main orchestration service that coordinates the executable search process
  - Uses PATH environment variable to search for executables
  - Handles both silent and normal output modes
  - Supports first-match and all-matches search modes
  - Returns appropriate exit codes (0 = success, 1 = failure)

- **PathOperations.cs**: File system operations for finding executables
  - `GetPaths()`: Parses colon-separated PATH environment variable into directory paths
  - `FilterExistingPaths()`: Filters out non-existent directories from the search paths
  - `FindFile()`: Searches for a single executable in all directories
  - `FindFiles()`: Searches for multiple executables and aggregates results
  - Uses `System.IO.Abstractions.IFileSystem` for testable file system operations

### Infrastructure Services
- **EnvironmentService.cs**: Abstracts environment variable access
  - `GetSearchPath()`: Safely retrieves the PATH environment variable
  - Returns `Result<NonNullableString>` for functional error handling

- **ConsoleLogger.cs**: Simple implementation of `ILogger` interface
  - Writes log messages to the console
  - Used for outputting search results and error messages

## Service Dependencies

The services follow a dependency flow:
1. **CommandLineParser** → **CommandLineProcessor** → **Settings**
2. **EnvironmentService** → **PathOperations** → **SearchService**
3. All services use the **Result Pattern** for error handling without exceptions

## Design Patterns

- **Functional Programming**: All services return `Result<T>` instead of throwing exceptions
- **Dependency Injection**: Uses `IFileSystem` and `ILogger` abstractions for testability
- **Static Methods**: Most services are static classes with pure functions
- **Separation of Concerns**: Each service has a single, well-defined responsibility