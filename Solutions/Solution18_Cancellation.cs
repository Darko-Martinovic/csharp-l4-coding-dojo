using CodingTasks.Core;

namespace CodingTasks.Solutions.S18;

public sealed class Solution18_Cancellation : ISolution
{
    public int Id => 18;

    private static int _handled;

    // Cooperative cancellation: thread the token through, poll it each iteration,
    // and pass it to awaitables so cancellation throws OperationCanceledException.
    private static async Task ProcessAll(IEnumerable<int> items, CancellationToken ct)
    {
        foreach (var item in items)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(50, ct);
            _handled = item;
        }
    }

    public void Run()
    {
        _handled = 0;
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(120);   // cancel after ~2 iterations

        try
        {
            ProcessAll(Enumerable.Range(1, 100), cts.Token).GetAwaiter().GetResult();
            Console.WriteLine("completed all items");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"cancelled cooperatively after handling item #{_handled}");
        }
    }
}
