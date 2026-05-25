using CodingTasks.Core;

namespace CodingTasks.Solutions.S09;

public sealed class Solution09_MultipleEnumeration : ISolution
{
    public int Id => 9;

    public void Run()
    {
        int calls = 0;
        IEnumerable<int> Query() => Enumerable.Range(1, 3).Select(x => { calls++; return x; });

        // Materialize once at the boundary, then operate on the snapshot: Count
        // is O(1) on the list and the foreach reuses the same buffer.
        var snapshot = Query().ToList();
        Console.WriteLine($"count = {snapshot.Count}");
        foreach (var i in snapshot) { _ = i; }

        Console.WriteLine($"projection ran {calls} times (evaluated exactly once)");
    }
}
