using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task13_RecordsWith : ICodingTask
{
    public int Id => 13;
    public string Title => "Value equality and non-destructive copies";
    public string Category => "Records & Patterns";

    public string Description => """
        Money is a class. Two instances with identical fields compare unequal
        (reference equality), and "copying then editing" mutates the original
        because both names point at the same object.

        Redesign Money so that equality is value-based and you can produce a
        modified copy without touching the source.
        """;

    public string StarterCode => """
        using System;

        class Money {
            public decimal Amount;
            public string Currency;
            public Money(decimal a, string c) { Amount = a; Currency = c; }
        }

        var a = new Money(10, "USD");
        var b = new Money(10, "USD");
        Console.WriteLine(a == b);     // False (reference equality)

        var c = a;                     // same reference
        c.Amount = 20;                 // also changes a!
        Console.WriteLine(a.Amount);   // 20
        """;

    public string SolutionExplanation => """
        A positional record gives you, for free: value-based Equals/== and
        GetHashCode (so a == b is true when fields match), an immutable shape
        with init-only properties, a readable ToString, Deconstruct, and the
        `with` expression for non-destructive mutation — `a with { Amount = 20 }`
        returns a new Money and leaves `a` untouched. This is exactly the
        immutable value-object you want for money.
        """;

    public string SolutionCode => """
        record Money(decimal Amount, string Currency);

        var a = new Money(10, "USD");
        var b = new Money(10, "USD");
        Console.WriteLine(a == b);            // True  (value equality)

        var c = a with { Amount = 20 };       // new instance
        Console.WriteLine(a.Amount);          // 10  (unchanged)
        Console.WriteLine(c);                 // Money { Amount = 20, Currency = USD }

        var (amount, currency) = c;           // deconstruction
        """;
}
