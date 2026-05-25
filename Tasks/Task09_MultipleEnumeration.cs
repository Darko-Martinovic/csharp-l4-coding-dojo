using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task09_MultipleEnumeration : ICodingTask
{
    public int Id => 9;
    public string Title => "Possible multiple enumeration of IEnumerable";
    public string Category => "Iterators";

    public string Description => """
        Process() receives an IEnumerable<int>, calls Count() on it and then
        foreach-es it. Because the argument is a lazily-evaluated query with a
        side effect, it runs twice — doubling the work (and, with non-pure
        sources, producing inconsistent results).

        Fix it so the sequence is evaluated exactly once.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;
        using System.Linq;

        void Process(IEnumerable<int> items) {
            Console.WriteLine($"count = {items.Count()}");  // enumeration #1
            foreach (var i in items) Use(i);               // enumeration #2
        }

        int calls = 0;
        var query = Enumerable.Range(1, 3).Select(x => { calls++; return x; });
        Process(query);
        Console.WriteLine(calls);   // 6, expected 3
        """;

    public string SolutionExplanation => """
        A LINQ query is a recipe, not a result: every time you enumerate it the
        pipeline re-runs. Count() walks it once and foreach walks it again, so
        the projection's side effect fires 6 times.

        Materialize the sequence once at the boundary (ToList()/ToArray()) and
        operate on the snapshot. Now Count is O(1) on the list and the foreach
        reuses the same buffer. As a rule, accept IEnumerable but realize it
        before enumerating more than once.
        """;

    public string SolutionCode => """
        void Process(IEnumerable<int> items) {
            var snapshot = items as IReadOnlyCollection<int> ?? items.ToList();
            Console.WriteLine($"count = {snapshot.Count}");  // no re-enumeration
            foreach (var i in snapshot) Use(i);
        }
        """;
}
