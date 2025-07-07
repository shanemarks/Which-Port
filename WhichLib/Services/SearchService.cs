
using System.IO.Abstractions;
using WhichLib.Data;
using WhichLib.Interfaces;

namespace WhichLib.Services;

public class SearchService
{
    public static void Search(ILogger logger, string pathToSearch, Settings settings, IFileSystem fileSystem)
    {
            var result = PathOperations.FindFiles(
                PathOperations.FilterExistingPaths(
                    PathOperations.GetPaths(
                        new NonNullableString(pathToSearch)), 
                    fileSystem)
            ,settings.Files, fileSystem);

            if (!result.IsSuccessful)
            {
                logger.Log(result.Error);
                return;
            }

            foreach (var match in result.Value.Matches)
            {
                foreach (var path in match.FoundPaths)
                {
                    logger.Log(path.Value);
                    if (!settings.ListAll)
                    {
                        break;
                    }
                }
            }
    }
}