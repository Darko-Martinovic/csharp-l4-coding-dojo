using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task07_MutableStruct : ICodingTask
{
    public int Id => 7;
    public string Title => "Mutating a struct in a List does nothing (or won't compile)";
    public string Category => "Value Types";

    public string Description => """
        Counter is a mutable struct stored in a List. The code tries to bump
        the first element in place, but `list[0].Increment()` is a compile
        error (CS1612) — and even the workaround that compiles leaves the list
        unchanged.

        Explain the value-type semantics at play and provide a correct,
        idiomatic design.
        """;

    public string StarterCode => """
        struct Counter {
            public int Value;
            public void Increment() => Value++;
        }

        var list = new List<Counter> { new Counter() };
        list[0].Increment();              // CS1612: modifying a temporary copy
        Console.WriteLine(list[0].Value); // would still be 0 anyway
        """;

    public string SolutionExplanation => """
        list[0] returns a *copy* of the struct (the indexer getter returns by
        value). Calling Increment() on that throwaway copy can't change the
        stored element, so the compiler blocks it outright.

        The senior answer is to make value types immutable: expose operations
        that return a new value, then reassign. This makes the copy semantics
        explicit and bug-resistant:
            var c = list[0];
            list[0] = c.Increment();
        (`readonly struct` enforces immutability; a `readonly record struct`
        gives you that plus equality for free.)
        """;

    public string SolutionCode => """
        readonly struct Counter {
            public int Value { get; }
            public Counter(int value) => Value = value;
            public Counter Increment() => new(Value + 1);   // returns a new value
        }

        var list = new List<Counter> { new Counter() };
        list[0] = list[0].Increment();    // copy out, transform, store back
        Console.WriteLine(list[0].Value); // 1
        """;
}
