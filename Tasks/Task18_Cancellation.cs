using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task18_Cancellation : ICodingTask
{
    public int Id => 18;
    public string Title => "Cooperative cancellation with CancellationToken";
    public string Category => "Async & Tasks";

    public string Description => """
        ProcessAll runs a long loop but offers no way to stop it — a caller who
        wants to cancel has to wait it out (or abort the thread). Make it
        accept a CancellationToken and stop promptly and correctly when
        cancellation is requested.
        """;

    public string StarterCode => """
        async Task ProcessAll(IEnumerable<int> items) {
            foreach (var item in items) {
                await Task.Delay(50);    // some work
                Handle(item);
            }                            // no way to cancel
        }
        """;

    public string SolutionExplanation => """
        Cancellation in .NET is cooperative: thread the CancellationToken
        through and poll it. Pass the token to awaitables (Task.Delay(…, ct))
        and call ct.ThrowIfCancellationRequested() each iteration. When the
        linked CancellationTokenSource is cancelled, the operation throws
        OperationCanceledException, which the caller catches to observe the
        cancellation — distinct from a failure. Make the token the last
        parameter by convention.
        """;

    public string SolutionCode => """
        async Task ProcessAll(IEnumerable<int> items, CancellationToken ct) {
            foreach (var item in items) {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(50, ct);   // also observes cancellation
                Handle(item);
            }
        }

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(120);
        try { await ProcessAll(items, cts.Token); }
        catch (OperationCanceledException) { /* expected on cancel */ }
        """;
}
