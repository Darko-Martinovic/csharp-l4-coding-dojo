using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task25_GenericConstraints : ICodingTask
{
    public int Id => 25;
    public string Title => "Generic method needs a constraint to compile";
    public string Category => "Generics & LINQ";

    public string Description => """
        Max<T> compares two values with `>`, but the unconstrained T is just
        `object` to the compiler, which has no `>` operator — so it won't
        compile. Constrain T so the comparison is legal and works for ints,
        strings, dates, and any orderable type.
        """;

    public string StarterCode => """
        T Max<T>(T a, T b) {
            return a > b ? a : b;   // CS0019: '>' cannot be applied to 'T'
        }
        """;

    public string SolutionExplanation => """
        A type parameter exposes only what its constraints guarantee. Add
        `where T : IComparable<T>` and compare with a.CompareTo(b) > 0 — now any
        type that defines a natural order (int, string, DateTime, your own
        IComparable<T> types) works. Other common constraints: `where T :
        struct` (value type), `where T : class` (reference type), `where T :
        new()` (parameterless ctor), `where T : SomeBase`/`SomeInterface`, and
        `where T : notnull`. List multiple separated by commas.
        """;

    public string SolutionCode => """
        T Max<T>(T a, T b) where T : IComparable<T>
            => a.CompareTo(b) >= 0 ? a : b;

        Max(3, 7);                  // 7
        Max("apple", "banana");     // banana
        Max(DateTime.MinValue, DateTime.MaxValue);
        """;
}
