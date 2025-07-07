# Data Layer

This folder contains immutable data types and value objects that represent the core domain models for the Which application.

## Files

### Command Line Types (`CommandLineTypes.cs`)
- **RawArguments**: Immutable wrapper for raw command line arguments from the environment
- **ParsedArguments**: Contains parsed command options and file names to search
- **CommandOptions**: Wrapper for command line option strings (e.g., "-a", "-s")
- **FileNamesToSearch**: Type-safe collection of executable file names to search for

### Type-Safe Strings (`NonNullableString.cs`)
- **NonNullableString**: A value type that wraps strings and prevents null values at compile time
- Provides string operations like `StartsWith`, `Contains`, `Split` with null safety
- Includes factory methods for graceful null handling

### Path Types (`PathTypes.cs`)
- **DirectoryPath**: Type-safe wrapper for directory paths
- **FilePath**: Type-safe wrapper for file paths  
- **PathCollection**: Immutable collection of directory paths
- **FileCollection**: Immutable collection of file paths

### Result Pattern (`Result.cs`)
- **Result\<T\>**: Custom result type for error handling without exceptions
- Encapsulates either a successful value or an error message
- Enables functional programming patterns for error handling

### Search Results (`SearchResult.cs`)
- **ExecutablePath**: Represents a path to an executable file
- **SearchMatch**: Associates a file name with all found executable paths
- **SearchResults**: Collection of all search matches with summary statistics

### Settings (`Settings.cs`, `WhichSettings.cs`)
- **Settings**: Main settings container wrapping WhichOptions
- **WhichOptions**: Core configuration object containing output mode, search mode, and files to search
- **OutputMode**: Enum for output behavior (Normal, Silent, Verbose)
- **SearchMode**: Enum for search behavior (FirstMatch, AllMatches)

## Design Principles

All types in this folder follow these patterns:
- **Immutable**: All data types are readonly records/structs
- **Type Safety**: Strong typing prevents common errors
- **Factory Methods**: Static factory methods for common construction scenarios
- **Null Safety**: NonNullableString prevents null reference exceptions
- **Value Semantics**: Equality based on value, not reference