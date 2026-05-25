using CodingTasks.Core;

namespace CodingTasks.Solutions.S19;

// A custom exception carrying typed data (StatusCode).
public sealed class ServiceException(int statusCode, string message)
    : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public sealed class Solution19_CustomExceptionFilter : ISolution
{
    public int Id => 19;

    private static void Call(int statusCode)
        => throw new ServiceException(statusCode, $"service returned {statusCode}");

    // The `when` filter decides whether to enter the catch WITHOUT unwinding the
    // stack — non-429s skip it entirely and propagate with their original trace.
    private static string Attempt(int statusCode)
    {
        try
        {
            Call(statusCode);
            return "ok";
        }
        catch (ServiceException ex) when (ex.StatusCode == 429)
        {
            return $"retryable ({ex.StatusCode}) -> retrying";
        }
    }

    public void Run()
    {
        Console.WriteLine($"429: {Attempt(429)}");

        try
        {
            Console.WriteLine($"500: {Attempt(500)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"500: not caught by filter -> propagated as {ex.GetType().Name}");
        }
    }
}
