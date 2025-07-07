using System.Collections.Immutable;
using WhichLib.Data;

namespace WhichLib.Services;

public static class CommandLineParser
{
    public static Result<ParsedArguments> Parse(RawArguments rawArgs)
    {
        if (rawArgs.IsEmpty)
        {
            return Result<ParsedArguments>.Failure("No arguments provided");
        }

        var firstArg = rawArgs.Args[0];
        var (options, fileArgs) = firstArg.StartsWith("-") 
            ? (new CommandOptions(firstArg), rawArgs.Args.Skip(1).ToImmutableArray())
            : (CommandOptions.Empty, rawArgs.Args);

        var filesToSearch = FileNamesToSearch.FromNonNullableStrings(fileArgs);
        
        return Result<ParsedArguments>.Success(new ParsedArguments(options, filesToSearch));
    }
}