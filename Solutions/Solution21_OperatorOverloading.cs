using CodingTasks.Core;

namespace CodingTasks.Solutions.S21;

// == and != come in pairs and must agree with Equals/GetHashCode, or `a == b`
// and a.Equals(b) can disagree and hash collections break.
public readonly struct Vector2(double x, double y) : IEquatable<Vector2>
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 v, double k) => new(v.X * k, v.Y * k);
    public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
    public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);
    public bool Equals(Vector2 o) => X == o.X && Y == o.Y;
    public override bool Equals(object? o) => o is Vector2 v && Equals(v);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";
}

public sealed class Solution21_OperatorOverloading : ISolution
{
    public int Id => 21;

    public void Run()
    {
        var a = new Vector2(1, 2);
        var b = new Vector2(3, 4);
        Console.WriteLine($"a + b   = {a + b}");
        Console.WriteLine($"b - a   = {b - a}");
        Console.WriteLine($"a * 2   = {a * 2}");
        Console.WriteLine($"a == b  = {a == b}");
        Console.WriteLine($"a == (1,2) = {a == new Vector2(1, 2)}");
    }
}
