using WhichLib.Interfaces;

namespace WhichLib.Services;

public class ConsoleLogger:ILogger
{
    public void Log(string message)
    {
       Console.WriteLine(message);
    }
}