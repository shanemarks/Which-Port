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

            foreach (var files in result.Value)
            {
                if (!files.IsDefaultOrEmpty)
                {
                    foreach (var file in files)
                    {
                        logger.Log(file.ToString());
                        if (!settings.ListAll)
                        {
                            break;
                        }
                    }
                }
            }
    }
}