# Interfaces

This folder contains interface definitions that enable dependency injection and testability throughout the WhichLib library.

## Files

### ILogger.cs
- **ILogger**: Simple logging abstraction with a single `Log(string message)` method
- Enables the application to output messages without being coupled to specific logging implementations
- Used by `SearchService` to output search results and error messages
- Default implementation is `ConsoleLogger` which writes to the console
- Can be easily mocked for unit testing or replaced with alternative logging implementations

## Design Benefits

- **Testability**: Services can be tested with mock implementations
- **Flexibility**: Logging behavior can be changed without modifying service code  
- **Separation of Concerns**: Business logic is decoupled from infrastructure concerns
- **SOLID Principles**: Follows Dependency Inversion Principle by depending on abstractions