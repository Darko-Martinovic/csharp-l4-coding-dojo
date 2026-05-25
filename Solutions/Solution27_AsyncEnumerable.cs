using System.Runtime.CompilerServices;
using CodingTasks.Core;

namespace CodingTasks.Solutions.S27;

public sealed class Solution27_AsyncEnumerable : ISolution
{
    public int Id => 27;

    // `async` + `yield return` produces each page lazily; the consumer receives
    // it immediately via `await foreach`. [EnumeratorCancellation] lets a token
    // passed to WithCancellation flow into the iterator and the awaited Delay.
    private static async IAsyncEnumerable<int> GetPages(
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        for (int p = 1; p <= 3; p++)
        {
            await Task.Delay(20, ct);
            yield return p;
        }
    }

    public void Run()
    {
        ConsumeAsync().GetAwaiter().GetResult();

        static async Task ConsumeAsync()
        {
            await foreach (var page in GetPages())
                Console.WriteLine($"received page {page} (processed as it streamed in)");
        }
    }
}
