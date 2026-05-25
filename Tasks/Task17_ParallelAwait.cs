using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task17_ParallelAwait : ICodingTask
{
    public int Id => 17;
    public string Title => "Awaiting independent tasks one at a time";
    public string Category => "Async & Tasks";

    public string Description => """
        FetchAll awaits each independent async call inside a loop, so the calls
        run strictly one after another and the total latency is the sum of all
        of them. These operations don't depend on each other.

        Refactor so they run concurrently and the method returns as soon as the
        slowest one finishes.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;

        async Task<int[]> FetchAll(int[] ids) {
            var results = new List<int>();
            foreach (var id in ids)
                results.Add(await FetchAsync(id));   // serialized!
            return results.ToArray();
        }
        """;

    public string SolutionExplanation => """
        `await` inside the loop suspends until each call completes before
        starting the next, so latencies add up. Since the calls are
        independent, start them all first (each FetchAsync(id) returns a hot
        Task immediately), collect the tasks, then `await Task.WhenAll(...)`.
        Total time becomes roughly the single slowest call, and WhenAll
        preserves result order matching the input.
        """;

    public string SolutionCode => """
        async Task<int[]> FetchAll(int[] ids) {
            var tasks = ids.Select(FetchAsync);     // all started, not awaited
            return await Task.WhenAll(tasks);       // run concurrently
        }
        """;
}
