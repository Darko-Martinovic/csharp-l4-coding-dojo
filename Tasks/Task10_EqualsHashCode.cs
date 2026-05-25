using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task10_EqualsHashCode : ICodingTask
{
    public int Id => 10;
    public string Title => "Override Equals but not GetHashCode breaks HashSet";
    public string Category => "Equality";

    public string Description => """
        Point overrides Equals for value semantics but leaves GetHashCode at
        the default identity implementation. As a result a HashSet can't find
        an equal-but-different instance: Contains returns false even though
        Equals would say true.

        Implement equality correctly so the type behaves in hash-based
        collections.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        class Point {
            public int X, Y;
            public override bool Equals(object? o)
                => o is Point p && p.X == X && p.Y == Y;
            // GetHashCode NOT overridden  -> CS0659, and hashing is broken
        }

        var set = new HashSet<Point> { new() { X = 1, Y = 2 } };
        Console.WriteLine(set.Contains(new() { X = 1, Y = 2 }));  // False
        """;

    public string SolutionExplanation => """
        Hash-based collections first bucket by GetHashCode, then compare with
        Equals only within a bucket. Two equal Points return different default
        hash codes, land in different buckets, and never get compared — so
        Contains misses.

        The contract: if a.Equals(b) then a.GetHashCode() == b.GetHashCode().
        Override GetHashCode from the same fields (HashCode.Combine), and
        implement IEquatable<Point> to avoid boxing/casting on each compare.
        """;

    public string SolutionCode => """
        sealed class Point : IEquatable<Point> {
            public int X { get; init; }
            public int Y { get; init; }

            public bool Equals(Point? p) => p is not null && p.X == X && p.Y == Y;
            public override bool Equals(object? o) => Equals(o as Point);
            public override int GetHashCode() => HashCode.Combine(X, Y);
        }
        // (a positional `record` would generate all of this automatically.)
        """;
}
