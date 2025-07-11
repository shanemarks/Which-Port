
using System.IO.Abstractions;
using WhichLib.Data;
using WhichLib.Interfaces;

namespace WhichLib.Services;

public class SearchService
{
    public static int Search(ILogger logger, string pathToSearch, Settings settings, IFileSystem fileSystem)
    {
            var result = PathOperations.FindFiles(
                PathOperations.FilterExistingPaths(
                    PathOperations.GetPaths(
                        new NonNullableString(pathToSearch)), 
                    fileSystem)
            ,settings.Options.FilesToSearch.Names, fileSystem);

            if (!result.IsSuccessful)
            {
                // In silent mode, don't output errors to logger
                if (settings.Options.Output != OutputMode.Silent)
                {
                    logger.Log(result.Error);
                }
                return 1; // Exit code 1 indicates failure
            }

            bool foundAnyExecutables = false;
            foreach (var match in result.Value.Matches)
            {
                foreach (var path in match.FoundPaths)
                {
                    foundAnyExecutables = true;
                    
                    // In silent mode, don't output the paths
                    if (settings.Options.Output != OutputMode.Silent)
                    {
                        logger.Log(path.Value);
                    }
                    
                    if (settings.Options.Search == SearchMode.FirstMatch)
                    {
                        break;
                    }
                }
            }
            
            // Return appropriate exit code
            return foundAnyExecutables ? 0 : 1; // 0 = success, 1 = not found
    }
}