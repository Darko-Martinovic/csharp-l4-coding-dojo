using CodingTasks.Core;

namespace CodingTasks.Solutions.S13;

// A positional record gives value equality, init-only immutability, ToString,
// Deconstruct and the `with` expression for non-destructive copies — for free.
public record Money(decimal Amount, string Currency);

public sealed class Solution13_RecordsWith : ISolution
{
    public int Id => 13;

    public void Run()
    {
        var a = new Money(10, "USD");
        var b = new Money(10, "USD");
        Console.WriteLine($"a == b (value equality): {a == b}");

        var c = a with { Amount = 20 };
        Console.WriteLine($"a unchanged after `with`: a.Amount = {a.Amount}");
        Console.WriteLine($"c = {c}");

        var (amount, currency) = c;
        Console.WriteLine($"deconstructed: amount={amount}, currency={currency}");
    }
}
