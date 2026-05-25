using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task08_BoxingEquality : ICodingTask
{
    public int Id => 8;
    public string Title => "Why does == on two boxed ints return false?";
    public string Category => "Value Types";

    public string Description => """
        Two ints with the same value are boxed into object references and
        compared. `==` returns false while `.Equals` returns true. Explain the
        difference and show how to compare boxed values for equality reliably.
        """;

    public string StarterCode => """
        using System;

        object a = 5;
        object b = 5;

        Console.WriteLine(a == b);        // False  (?!)
        Console.WriteLine(a.Equals(b));   // True
        """;

    public string SolutionExplanation => """
        Boxing each int allocates a *separate* object on the heap, so `a` and
        `b` are different references. For `object`, the `==` operator binds to
        reference equality (object.ReferenceEquals), which is false.

        Equals is virtual: object.Equals dispatches to Int32.Equals, which
        compares the unboxed values — hence true. To compare values regardless
        of boxing, use .Equals or EqualityComparer<object>.Default.Equals, or
        unbox to the known type and use the value-type ==.
        """;

    public string SolutionCode => """
        object a = 5;
        object b = 5;

        Console.WriteLine(a == b);                                   // False (reference)
        Console.WriteLine(a.Equals(b));                             // True  (value)
        Console.WriteLine(EqualityComparer<object>.Default.Equals(a, b)); // True
        Console.WriteLine((int)a == (int)b);                       // True  (unboxed)
        """;
}
