namespace WhichLib.Data;

public struct Result<T> where T: struct
{
    public readonly T Value { get; }
    public readonly string Error { get; }
    
    public readonly bool IsSuccessful { get; }
    
    private Result (T value, string error, bool isSuccessful)
    {
        Value = value;
        Error = error;
        IsSuccessful = isSuccessful;
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, error, false);
    }
    
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, string.Empty,true);
    }
}