using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task06_IteratorDeferred : ICodingTask
{
    public int Id => 6;
    public string Title => "Iterator argument validation runs too late";
    public string Category => "Iterators";

    public string Description => """
        Squares() validates its argument and then yields. A caller passes null
        and expects an immediate ArgumentNullException — but the exception only
        fires later, when the sequence is first enumerated (and never, if it
        isn't).

        Fix the method so validation happens eagerly at call time while the
        iteration stays lazy.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        IEnumerable<int> Squares(IEnumerable<int> src) {
            if (src is null)                       // deferred: body doesn't run
                throw new ArgumentNullException(nameof(src));   // until MoveNext
            foreach (var x in src)
                yield return x * x;
        }

        var q = Squares(null);          // does NOT throw here
        Console.WriteLine("created");   // this prints
        foreach (var s in q) { }        // throws only here
        """;

    public string SolutionExplanation => """
        Any method containing `yield` is compiled into a state machine whose
        body — including your guard clause — does not execute until the first
        MoveNext(). So validation is deferred along with everything else.

        The standard fix is to split it: a normal (non-iterator) public method
        validates eagerly and returns the result of a private/local iterator
        function that does the lazy yielding.
        """;

    public string SolutionCode => """
        IEnumerable<int> Squares(IEnumerable<int> src) {
            ArgumentNullException.ThrowIfNull(src);   // runs immediately
            return Iterator(src);

            static IEnumerable<int> Iterator(IEnumerable<int> s) {
                foreach (var x in s)
                    yield return x * x;               // still lazy
            }
        }
        """;
}
