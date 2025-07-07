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
        

        bool all = options.ToString().Contains("a");
        bool silent = options.ToString().Contains("s");
        return Result<Settings>.Success(new Settings(silent, all, filesToSearch.Names));
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