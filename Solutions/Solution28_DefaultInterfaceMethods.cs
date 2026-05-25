using CodingTasks.Core;

namespace CodingTasks.Solutions.S28;

// A default interface method ships a body, so adding LogError is non-breaking —
// implementers that don't override it inherit the default. It dispatches through
// the interface type, so call it via an ILogger reference.
public interface ILogger
{
    void Log(string message);
    void LogError(string message) => Log($"ERROR: {message}");
}

public sealed class ConsoleLogger : ILogger
{
    public void Log(string m) => Console.WriteLine(m);
}

public sealed class Solution28_DefaultInterfaceMethods : ISolution
{
    public int Id => 28;

    public void Run()
    {
        ILogger logger = new ConsoleLogger();
        logger.Log("starting up");
        logger.LogError("disk full");   // uses the default implementation
    }
}
