using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Security;
using WhichLib.Data;

namespace WhichLib.Services;

public static class PathOperations
{

    public static Result<SearchResults> FindFiles(Result<PathCollection> paths,
        ImmutableArray<NonNullableString> files, IFileSystem fileSystem)
    {
        if (!paths.IsSuccessful)
        {
            return Result<SearchResults>.Failure(paths.Error);
        }
        
        List<SearchMatch> foundFiles = new List<SearchMatch>();
        foreach (var file in files)
        {
            var result = FindFile(paths, file, fileSystem);
            if (result.IsSuccessful)
            {
                var executablePaths = result.Value.Files.Select(filePath => new ExecutablePath(filePath.ToString())).ToImmutableArray();
                foundFiles.Add(new SearchMatch(file.ToString(), executablePaths));
            }
        }
        
        if (foundFiles.Count == 0)
        {
            return Result<SearchResults>.Failure("could not find file");
        }
        
        return Result<SearchResults>.Success(new SearchResults(foundFiles.ToImmutableArray()));

    }
    public static Result<FileCollection> FindFile(Result<PathCollection> paths, NonNullableString file, IFileSystem fileSystem)
    {
        
        if (!paths.IsSuccessful)
        {
            return Result<FileCollection>.Failure(paths.Error);
        }

        List<FilePath> results = new List<FilePath>();
        foreach (var directoryPath in paths.Value.Paths)
        {
            string fullpath = fileSystem.Path.Combine(directoryPath.ToString(), file.ToString());
            if (fileSystem.File.Exists(fullpath))
            {
                results.Add(new FilePath(new NonNullableString(fullpath)));
            }
        }

        if (results.Count > 0)
        {
            return Result<FileCollection>.Success(new FileCollection(results.ToImmutableArray()));
        }
        return Result<FileCollection>.Failure("could not find file");
    }
    public static Result<PathCollection> FilterExistingPaths(Result<PathCollection> paths,
        IFileSystem fileSystem)
    {
        //don't do anything if input is invalid
        if (!paths.IsSuccessful)
        {
            return paths;
        }
        var filtered = paths.Value.Paths.Where(x => fileSystem.Directory.Exists(x.ToString())).ToImmutableArray();
        return Result<PathCollection>.Success(new PathCollection(filtered));
    }
    public static Result<PathCollection> GetPaths(NonNullableString semicolonSeparatedPaths)
    {
        string[] results = semicolonSeparatedPaths.ToString().Split(":");
        var validPaths = results.Where(x =>
        {
            try
            {
                //if its a valid path it will return true otherwise it will throw an exception here.
                Path.GetFullPath(x.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }).ToArray();

        if (validPaths.Length == 0)
        {
            return Result<PathCollection>.Failure("No Paths Specified");
        }
        
        return Result<PathCollection>.Success(PathCollection.FromStrings(validPaths));
    }
}