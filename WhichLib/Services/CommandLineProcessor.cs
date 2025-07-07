using System.Collections.Immutable;
using System.Security;
using System.Text.RegularExpressions;
using WhichLib.Data;

namespace WhichLib.Services;

public static class CommandLineProcessor
{

    public static Result<Settings> OptionsToSettings(CommandOptions options, FileNamesToSearch filesToSearch)
    {
        var validationResult = ValidateOption(options.Value);
        if (!validationResult.IsValid)
        {
            return Result<Settings>.Failure($"which: illegal option -- {validationResult.InvalidCharacter}");
        }
        

        var outputMode = options.ToString().Contains("s") ? OutputMode.Silent : OutputMode.Normal;
        var searchMode = options.ToString().Contains("a") ? SearchMode.AllMatches : SearchMode.FirstMatch;
        
        var whichOptions = new WhichOptions(outputMode, searchMode, filesToSearch);
        return Result<Settings>.Success(new Settings(whichOptions));
    }
    public readonly record struct OptionValidationResult(bool IsValid, char? InvalidCharacter);
    
    public static OptionValidationResult ValidateOption(NonNullableString option)
    {
        string opt = option.ToString();
        if (opt == string.Empty) // this is a valid option.
        {
            return new OptionValidationResult(true, null);
        }
        if (opt.StartsWith("-"))
        {
            var optionChars = opt.Substring(1);
            foreach (char c in optionChars)
            {
                if (c != 'a' && c != 's')
                {
                    return new OptionValidationResult(false, c);
                }
            }
            return new OptionValidationResult(true, null);
        }

        return new OptionValidationResult(false, null);
    }
}