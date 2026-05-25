using CodingTasks.Core;

namespace CodingTasks.Solutions.S10;

// Equality from the same fields used by GetHashCode, plus IEquatable<Point> to
// avoid boxing/casting per compare — so the type behaves in hash-based sets.
public sealed class Point : IEquatable<Point>
{
    public int X { get; init; }
    public int Y { get; init; }
    public bool Equals(Point? p) => p is not null && p.X == X && p.Y == Y;
    public override bool Equals(object? o) => Equals(o as Point);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}

public sealed class Solution10_EqualsHashCode : ISolution
{
    public int Id => 10;

    public void Run()
    {
        var set = new HashSet<Point> { new() { X = 1, Y = 2 } };
        Console.WriteLine($"Contains equal instance: {set.Contains(new() { X = 1, Y = 2 })}");
        Console.WriteLine($"Adding a duplicate succeeds? {set.Add(new() { X = 1, Y = 2 })}");
        Console.WriteLine($"Set size stays: {set.Count}");
    }
}
