using CodingTasks.Core;

namespace CodingTasks.Solutions.S05;

// Async all the way down: RunAsync returns Task (awaitable, observable
// exceptions) and awaits instead of blocking on .Result.
public sealed class Service
{
    public async Task RunAsync()
    {
        var data = await GetAsync();
        Console.WriteLine($"received {data.Length} chars: \"{data}\"");
    }

    private static async Task<string> GetAsync()
    {
        await Task.Delay(10);
        return "payload";
    }
}

public sealed class Solution05_SyncOverAsync : ISolution
{
    public int Id => 5;

    public void Run()
    {
        // Synchronous bridge at the top level only (no captured context here,
        // so GetAwaiter().GetResult() is safe in a console entry point).
        new Service().RunAsync().GetAwaiter().GetResult();
        Console.WriteLine("completed without blocking the worker thread");
    }
}
