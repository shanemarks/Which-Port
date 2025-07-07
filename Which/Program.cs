// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.IO.Abstractions;
using WhichLib.Data;
using WhichLib.Services;

ImmutableArray<NonNullableString> strings = System.Environment.GetCommandLineArgs().Skip(1).Select((x=>new NonNullableString(x))).ToImmutableArray();
//No arguments pass in just exit 
if (strings.Length == 0) return;

//check for options
NonNullableString options = strings[0].ToString().StartsWith("-") ? strings[0] : NonNullableString.CreateOrEmpty("");

//get files
ImmutableArray<NonNullableString> files = options.ToString().Equals(string.Empty)
    ? strings
    : strings.Skip(1).ToImmutableArray();

//generate our settings
Result<Settings> s = CommandLineProcessor.OptionsToSettings(options,files);

if (!s.IsSuccessful)
{
    Console.WriteLine(s.Error);
    return;
}


SearchService.Search(new ConsoleLogger(), System.Environment.GetEnvironmentVariable("PATH") ?? throw new InvalidOperationException(),s.Value, new FileSystem());

