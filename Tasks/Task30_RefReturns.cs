using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task30_RefReturns : ICodingTask
{
    public int Id => 30;
    public string Title => "Return a reference to mutate a struct in place";
    public string Category => "Memory & Performance";

    public string Description => """
        Find returns a large struct from an array by value, so callers receive a
        copy: mutating it does nothing to the array, and each call copies all
        the fields. Change the API so the caller gets a reference to the actual
        element and can edit it in place with no copy.
        """;

    public string StarterCode => """
        struct Big { public long A, B, C, D; }

        Big Find(Big[] data, int i) => data[i];   // returns a COPY

        var item = Find(arr, 0);
        item.A = 99;              // edits the copy
        Console.WriteLine(arr[0].A);   // still 0
        """;

    public string SolutionExplanation => """
        A `ref` return hands back an alias to existing storage rather than a
        copy. Declare `ref Big Find(...)` and `return ref data[i];`, then capture
        it with a `ref` local: `ref var slot = ref Find(arr, 0);`. Now
        slot.A = 99 writes straight into arr[0] — no struct copy, no extra
        allocation. Use `ref readonly` / `in` to expose a no-copy *read-only*
        view of a large struct. (You can't ref-return a local or a temporary —
        only storage that outlives the call.)
        """;

    public string SolutionCode => """
        struct Big { public long A, B, C, D; }

        ref Big Find(Big[] data, int i) => ref data[i];   // returns a reference

        ref Big slot = ref Find(arr, 0);   // ref local: an alias to arr[0]
        slot.A = 99;                       // writes through to the array
        Console.WriteLine(arr[0].A);       // 99
        """;
}
