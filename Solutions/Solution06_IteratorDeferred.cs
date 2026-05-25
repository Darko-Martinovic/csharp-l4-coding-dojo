using CodingTasks.Core;

namespace CodingTasks.Solutions.S06;

public sealed class Solution06_IteratorDeferred : ISolution
{
    public int Id => 6;

    // A normal method validates eagerly, then returns a private iterator that
    // does the lazy yielding — so the guard runs at call time, not first MoveNext.
    private static IEnumerable<int> Squares(IEnumerable<int> src)
    {
        ArgumentNullException.ThrowIfNull(src);
        return Iterator(src);

        static IEnumerable<int> Iterator(IEnumerable<int> s)
        {
            foreach (var x in s)
                yield return x * x;
        }
    }

    public void Run()
    {
        try
        {
            _ = Squares(null!);   // eager: throws right here, before enumeration
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("null rejected eagerly at call site (good)");
        }

        var q = Squares([1, 2, 3, 4]);
        Console.WriteLine("sequence created lazily; enumerating now:");
        Console.WriteLine(string.Join(", ", q));   // 1, 4, 9, 16
    }
}
