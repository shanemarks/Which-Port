using System.Collections.Immutable;

namespace WhichLib.Data;

public readonly record struct ExecutablePath(string Value);

public readonly record struct SearchMatch(
    string FileName,
    ImmutableArray<ExecutablePath> FoundPaths);

public readonly record struct SearchResults(
    ImmutableArray<SearchMatch> Matches)
{
    public bool IsEmpty => Matches.IsEmpty;
    public int TotalMatches => Matches.Sum(m => m.FoundPaths.Length);
    
    public static SearchResults Empty => new(ImmutableArray<SearchMatch>.Empty);
}