using CodingTasks.Core;

namespace CodingTasks.Solutions.S14;

public sealed record Circle(double Radius);
public sealed record Rectangle(double W, double H);

public sealed class Solution14_PatternMatching : ISolution
{
    public int Id => 14;

    // Relational + logical patterns express ranges directly; once `< 0` is ruled
    // out, `< 15` means [0, 15), so no redundant lower bounds are needed.
    private static string Describe(int t) => t switch
    {
        < 0 => "freezing",
        < 15 => "cold",
        < 25 => "mild",
        _ => "hot",
    };

    // Type patterns with property sub-patterns bind and test in one step.
    private static double Area(object shape) => shape switch
    {
        Circle { Radius: var r } => Math.PI * r * r,
        Rectangle { W: var w, H: var h } => w * h,
        null => throw new ArgumentNullException(nameof(shape)),
        _ => throw new ArgumentException("unknown shape"),
    };

    public void Run()
    {
        foreach (var t in new[] { -5, 8, 20, 33 })
            Console.WriteLine($"{t,3}°C -> {Describe(t)}");

        Console.WriteLine($"Circle(2) area    = {Area(new Circle(2)):F2}");
        Console.WriteLine($"Rectangle(3,4) area = {Area(new Rectangle(3, 4)):F2}");
    }
}
