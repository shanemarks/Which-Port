using WhichLib.Data;

namespace WhichLib.Services;

public static class EnvironmentService
{
    public static Result<NonNullableString> GetSearchPath()
    {
        var pathValue = Environment.GetEnvironmentVariable("PATH");
        
        if (string.IsNullOrEmpty(pathValue))
        {
            return Result<NonNullableString>.Failure("PATH environment variable is not set or empty");
        }
        
        return Result<NonNullableString>.Success(new NonNullableString(pathValue));
    }
}