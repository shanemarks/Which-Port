using System.Collections.Immutable;

namespace WhichLib.Data;

public struct Settings
{
    public readonly bool SilentMode { get; } //-s
    public readonly bool ListAll { get; } //-a

    public readonly ImmutableArray<NonNullableString> Files { get; }

    public Settings(bool silentMode, bool listAll, ImmutableArray<NonNullableString> files)
    {
        SilentMode = silentMode;
        ListAll = listAll;
        Files = files;
    }
}