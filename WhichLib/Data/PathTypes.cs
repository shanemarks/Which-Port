using System.Collections.Immutable;

namespace WhichLib.Data;

public readonly record struct DirectoryPath(NonNullableString Value)
{
    public override string ToString() => Value.ToString();
}

public readonly record struct FilePath(NonNullableString Value)
{
    public override string ToString() => Value.ToString();
}

public readonly record struct PathCollection(ImmutableArray<DirectoryPath> Paths)
{
    public bool IsEmpty => Paths.IsEmpty;
    public int Count => Paths.Length;
    
    public static PathCollection Empty => new(ImmutableArray<DirectoryPath>.Empty);
    
    public static PathCollection FromStrings(IEnumerable<string> paths) =>
        new(paths.Select(p => new DirectoryPath(new NonNullableString(p))).ToImmutableArray());
        
    public static PathCollection FromNonNullableStrings(ImmutableArray<NonNullableString> paths) =>
        new(paths.Select(p => new DirectoryPath(p)).ToImmutableArray());
}

public readonly record struct FileCollection(ImmutableArray<FilePath> Files)
{
    public bool IsEmpty => Files.IsEmpty;
    public int Count => Files.Length;
    
    public static FileCollection Empty => new(ImmutableArray<FilePath>.Empty);
    
    public static FileCollection FromStrings(IEnumerable<string> files) =>
        new(files.Select(f => new FilePath(new NonNullableString(f))).ToImmutableArray());
        
    public static FileCollection FromNonNullableStrings(ImmutableArray<NonNullableString> files) =>
        new(files.Select(f => new FilePath(f)).ToImmutableArray());
}