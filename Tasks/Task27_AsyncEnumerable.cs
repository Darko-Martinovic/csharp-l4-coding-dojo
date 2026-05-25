using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task27_AsyncEnumerable : ICodingTask
{
    public int Id => 27;
    public string Title => "Stream results with IAsyncEnumerable";
    public string Category => "Async & Tasks";

    public string Description => """
        GetPages buffers every page into a List before returning, so the caller
        can't start processing until the last page has arrived. Turn it into an
        async stream that yields each page as soon as it's fetched, supports
        cancellation, and is consumed with `await foreach`.
        """;

    public string StarterCode => """
        async Task<List<int>> GetPages() {        // buffers everything
            var all = new List<int>();
            for (int p = 1; p <= 3; p++) {
                await Task.Delay(50);             // fetch page p
                all.Add(p);
            }
            return all;                           // caller waits for ALL pages
        }
        """;

    public string SolutionExplanation => """
        Return `IAsyncEnumerable<T>` and combine `async` + `yield return`: each
        page is produced lazily and the consumer receives it immediately via
        `await foreach`, overlapping fetch and processing. Add a
        CancellationToken decorated with [EnumeratorCancellation] so the token
        passed to `WithCancellation(...)` flows into the iterator and into the
        awaited Task.Delay. This is the streaming counterpart to a paged API.
        """;

    public string SolutionCode => """
        async IAsyncEnumerable<int> GetPages(
            [EnumeratorCancellation] CancellationToken ct = default) {
            for (int p = 1; p <= 3; p++) {
                await Task.Delay(50, ct);     // fetch page p
                yield return p;               // hand it over right away
            }
        }

        await foreach (var page in GetPages().WithCancellation(ct))
            Process(page);                    // starts before later pages arrive
        """;
}
