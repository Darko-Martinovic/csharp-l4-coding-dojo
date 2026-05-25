using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task16_RaceCondition : ICodingTask
{
    public int Id => 16;
    public string Title => "Lost updates: non-atomic ++ across threads";
    public string Category => "Concurrency";

    public string Description => """
        A shared counter is incremented from many parallel iterations with
        `count++`. The final total is usually less than expected because `++`
        is read-modify-write, not atomic — two threads can read the same value
        and each write back the same +1, losing an update.

        Make the increment thread-safe.
        """;

    public string StarterCode => """
        using System;
        using System.Threading.Tasks;

        public class Program {
            public static void Main() {
                int count = 0;
                Parallel.For(0, 100_000, _ => {
                    count++;                 // race: read, +1, write are separate steps
                });
                Console.WriteLine(count);    // < 100000, non-deterministic
            }
        }
        """;

    public string SolutionExplanation => """
        `count++` compiles to load → increment → store. With many threads these
        steps interleave, so increments overwrite each other. Fixes:
        • Interlocked.Increment(ref count) — a single atomic CPU instruction,
          lock-free and fastest for a plain counter.
        • lock (gate) { count++; } — correct but heavier; use when you must
          update several fields together.
        Prefer Interlocked for one numeric field.
        """;

    public string SolutionCode => """
        int count = 0;
        Parallel.For(0, 100_000, _ => {
            Interlocked.Increment(ref count);   // atomic
        });
        Console.WriteLine(count);               // exactly 100000
        """;
}
