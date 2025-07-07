using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Security;
using WhichLib.Data;

namespace WhichLib.Services;

public static class PathOperations
{

    public static Result<ImmutableArray<ImmutableArray<NonNullableString>>> FindFiles(Result<ImmutableArray<NonNullableString>> paths,
        ImmutableArray<NonNullableString> files, IFileSystem fileSystem)
    {
        if (!paths.IsSuccessful)
        {
            return Result<ImmutableArray<ImmutableArray<NonNullableString>>>.Failure(paths.Error);
        }
        
        List<ImmutableArray<NonNullableString>> foundFiles = new   List<ImmutableArray<NonNullableString>>();
        foreach (var file in files)
        {
            var result = FindFile(paths, file, fileSystem);
            if (result.IsSuccessful)
            {
                foundFiles.Add(result.Value);   
            }
        }
        
        if (foundFiles.Count == 0)
        {
            return Result<ImmutableArray<ImmutableArray<NonNullableString>>>.Failure("could not find file");
        }
        
        return Result<ImmutableArray<ImmutableArray<NonNullableString>>>.Success(foundFiles.ToImmutableArray());

    }
    public static Result<ImmutableArray<NonNullableString>>FindFile(Result<ImmutableArray<NonNullableString>> paths, NonNullableString file, IFileSystem fileSystem)
    {
        
        if (!paths.IsSuccessful)
        {
            return Result<ImmutableArray<NonNullableString>>.Failure(paths.Error);
        }

        List<NonNullableString> results = new List<NonNullableString>();
        foreach (var s in paths.Value)
        {
            string fullpath = fileSystem.Path.Combine(s.ToString(),file.ToString());
            if (fileSystem.File.Exists(fullpath))
            {
                results.Add(new NonNullableString(fullpath));
            }
        }

        if (results.Count > 0)
        {
            return Result<ImmutableArray<NonNullableString>>.Success(results.ToImmutableArray());
        }
        return Result<ImmutableArray<NonNullableString>>.Failure("could not find file");
    }
    public static Result<ImmutableArray<NonNullableString>> FilterExistingPaths(Result<ImmutableArray<NonNullableString>> paths,
        IFileSystem fileSystem)
    {
        //don't do anything if input is invalid
        if (!paths.IsSuccessful)
        {
            return paths;
        }
        var filtered = paths.Value.Where(x => fileSystem.Directory.Exists(x.ToString())).ToImmutableArray();
        return Result<ImmutableArray<NonNullableString>>.Success(filtered);
    }
    public static Result<ImmutableArray<NonNullableString>> GetPaths(NonNullableString semicolonSeparatedPaths)
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
        }).Select((x => new NonNullableString(x))).ToImmutableArray();

        if (validPaths.Length == 0)
        {
            return Result<ImmutableArray<NonNullableString>>.Failure("No Paths Specified");
        }
        return Result<ImmutableArray<NonNullableString>>.Success(validPaths);
    }
}