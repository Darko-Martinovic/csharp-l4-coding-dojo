using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task21_OperatorOverloading : ICodingTask
{
    public int Id => 21;
    public string Title => "Make a value type support +, *, and ==";
    public string Category => "Operators";

    public string Description => """
        Vector2 can't be used with arithmetic or equality operators:
        `a + b`, `v * 2` and `a == b` all fail to compile. Add the operators so
        the type reads like a built-in numeric value — and keep == consistent
        with Equals/GetHashCode.
        """;

    public string StarterCode => """
        using System;

        struct Vector2 {
            public double X, Y;
            public Vector2(double x, double y) { X = x; Y = y; }
        }

        var a = new Vector2(1, 2);
        var b = new Vector2(3, 4);
        var sum = a + b;        // CS0019: operator '+' not defined
        var scaled = a * 2;     // CS0019
        bool eq = a == b;       // CS0019
        """;

    public string SolutionExplanation => """
        Operators are static methods named `operator +` etc. Define +, - and a
        scalar *; for == you must also define != (they come in pairs), and
        override Equals/GetHashCode so all three agree — otherwise `a == b` and
        a.Equals(b) can disagree and hash collections break. A `readonly record
        struct` would synthesise the equality members for you; here we show the
        explicit form. Operators should be pure and side-effect free.
        """;

    public string SolutionCode => """
        readonly struct Vector2(double x, double y) : IEquatable<Vector2> {
            public double X { get; } = x;
            public double Y { get; } = y;

            public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
            public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
            public static Vector2 operator *(Vector2 v, double k)  => new(v.X * k, v.Y * k);

            public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
            public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);

            public bool Equals(Vector2 o) => X == o.X && Y == o.Y;
            public override bool Equals(object? o) => o is Vector2 v && Equals(v);
            public override int GetHashCode() => HashCode.Combine(X, Y);
            public override string ToString() => $"({X}, {Y})";
        }
        """;
}
