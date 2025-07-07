using System.Collections.Immutable;

namespace WhichLib.Data;

public readonly record struct RawArguments(ImmutableArray<NonNullableString> Args)
{
    public bool IsEmpty => Args.IsEmpty;
    public int Count => Args.Length;
    
    public static RawArguments FromEnvironment() => 
        new(Environment.GetCommandLineArgs()
            .Skip(1)
            .Select(arg => new NonNullableString(arg))
            .ToImmutableArray());
            
    public static RawArguments Empty => new(ImmutableArray<NonNullableString>.Empty);
}

public readonly record struct ParsedArguments(
    CommandOptions Options,
    FileNamesToSearch FilesToSearch)
{
    public bool HasFiles => !FilesToSearch.IsEmpty;
}

public readonly record struct CommandOptions(NonNullableString Value)
{
    public bool IsEmpty => Value.IsEmpty;
    public static CommandOptions Empty => new(NonNullableString.CreateOrEmpty(""));
    
    public override string ToString() => Value.ToString();
}

public readonly record struct FileNamesToSearch(ImmutableArray<NonNullableString> Names)
{
    public bool IsEmpty => Names.IsEmpty;
    public int Count => Names.Length;
    
    public static FileNamesToSearch Empty => new(ImmutableArray<NonNullableString>.Empty);
    
    public static FileNamesToSearch FromStrings(IEnumerable<string> files) =>
        new(files.Select(f => new NonNullableString(f)).ToImmutableArray());
        
    public static FileNamesToSearch FromNonNullableStrings(ImmutableArray<NonNullableString> files) =>
        new(files);
}