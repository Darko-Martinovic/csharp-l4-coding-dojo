using System.Diagnostics;
using CodingTasks.Core;

namespace CodingTasks.Solutions.S17;

public sealed class Solution17_ParallelAwait : ISolution
{
    public int Id => 17;

    private static async Task<int> FetchAsync(int id)
    {
        await Task.Delay(100);   // simulate I/O
        return id * id;
    }

    // Start every (hot) task first, then await Task.WhenAll: the calls run
    // concurrently, total time ≈ the slowest one, and result order matches input.
    private static async Task<int[]> FetchAll(int[] ids)
        => await Task.WhenAll(ids.Select(FetchAsync));

    public void Run()
    {
        int[] ids = [1, 2, 3, 4, 5];
        var sw = Stopwatch.StartNew();
        var results = FetchAll(ids).GetAwaiter().GetResult();
        Console.WriteLine($"results=[{string.Join(",", results)}] in {sw.ElapsedMilliseconds} ms (ran concurrently)");
    }
}
