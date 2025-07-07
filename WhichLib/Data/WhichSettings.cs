namespace WhichLib.Data;

public enum OutputMode
{
    Normal,   // Default output behavior
    Silent,   // -s flag: no output, just exit codes
    Verbose   // Future: could add -v for verbose output
}

public enum SearchMode
{
    FirstMatch,  // Default: stop after finding first match
    AllMatches   // -a flag: show all matches in PATH
}

public readonly record struct WhichOptions(
    OutputMode Output,
    SearchMode Search,
    FileNamesToSearch FilesToSearch)
{
    // Convenience property for checking verbose mode
    public bool IsVerbose => Output == OutputMode.Verbose;
    
    // Factory methods for common scenarios
    public static WhichOptions Default(FileNamesToSearch files) =>
        new(OutputMode.Normal, SearchMode.FirstMatch, files);
        
    public static WhichOptions Silent(FileNamesToSearch files) =>
        new(OutputMode.Silent, SearchMode.FirstMatch, files);
        
    public static WhichOptions ShowAll(FileNamesToSearch files) =>
        new(OutputMode.Normal, SearchMode.AllMatches, files);
        
    public static WhichOptions SilentShowAll(FileNamesToSearch files) =>
        new(OutputMode.Silent, SearchMode.AllMatches, files);
}