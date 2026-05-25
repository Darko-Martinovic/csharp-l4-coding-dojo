using CodingTasks.Core;

namespace CodingTasks.Solutions.S16;

public sealed class Solution16_RaceCondition : ISolution
{
    public int Id => 16;

    public void Run()
    {
        // Interlocked.Increment is a single atomic instruction — lock-free and
        // correct for a plain counter, so no updates are lost.
        int count = 0;
        Parallel.For(0, 100_000, _ => Interlocked.Increment(ref count));
        Console.WriteLine($"count = {count} (always 100000)");
    }
}
