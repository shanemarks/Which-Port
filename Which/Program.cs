// See https://aka.ms/new-console-template for more information

using System.IO.Abstractions;
using WhichLib.Data;
using WhichLib.Services;

// Parse command line arguments
var rawArgs = RawArguments.FromEnvironment();
var parseResult = CommandLineParser.Parse(rawArgs);

if (!parseResult.IsSuccessful)
{
    Console.WriteLine(parseResult.Error);
    return 1;
}

// Convert to settings
var settingsResult = CommandLineProcessor.OptionsToSettings(
    parseResult.Value.Options, 
    parseResult.Value.FilesToSearch);

if (!settingsResult.IsSuccessful)
{
    Console.WriteLine(settingsResult.Error);
    return 1;
}

// Get PATH environment variable
var pathResult = EnvironmentService.GetSearchPath();
if (!pathResult.IsSuccessful)
{
    Console.WriteLine(pathResult.Error);
    return 1;
}

// Perform search
var exitCode = SearchService.Search(new ConsoleLogger(), pathResult.Value, settingsResult.Value, new FileSystem());
return exitCode;

