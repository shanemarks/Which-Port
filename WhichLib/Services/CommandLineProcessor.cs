using System.Collections.Immutable;
using System.Security;
using System.Text.RegularExpressions;
using WhichLib.Data;

namespace WhichLib.Services;

public static class CommandLineProcessor
{

    public static Result<Settings> OptionsToSettings(NonNullableString options, ImmutableArray<NonNullableString> files)
    {
        
        if (!ValidateOption(options))
        {
            return Result<Settings>.Failure("which: bad option - todo print bad option");
        }

        bool all = options.ToString().Contains("a");
        bool silent = options.ToString().Contains("s");
        return Result<Settings>.Success(new Settings(silent, all, files));
    }
    public static bool ValidateOption(NonNullableString option)
    {
        string opt = option.ToString();
        if (opt == string.Empty) // this is a valid option.
        {
            return true;
        }
        if (opt.StartsWith("-"))
        {
            Regex regex = new Regex(@"^(a|s|as|sa)*$");
            return  regex.IsMatch(opt.Substring(1)); 
        }

        return false;

    }
}