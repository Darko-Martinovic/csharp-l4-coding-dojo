using CodingTasks.Core;

namespace CodingTasks.Solutions.S22;

// Implicit when lossless and never-throwing (double labels a number); explicit
// when it changes meaning or can lose precision (a real scale change).
public readonly struct Celsius(double degrees)
{
    public double Degrees { get; } = degrees;
    public static implicit operator Celsius(double d) => new(d);
    public static explicit operator Fahrenheit(Celsius c) => new(c.Degrees * 9 / 5 + 32);
    public override string ToString() => $"{Degrees}°C";
}

public readonly struct Fahrenheit(double degrees)
{
    public double Degrees { get; } = degrees;
    public override string ToString() => $"{Degrees}°F";
}

public sealed class Solution22_ConversionOperators : ISolution
{
    public int Id => 22;

    public void Run()
    {
        Celsius c = 25;                       // implicit from double
        Fahrenheit f = (Fahrenheit)c;         // explicit conversion
        Console.WriteLine($"implicit: 25 -> {c}");
        Console.WriteLine($"explicit: {c} -> {f}");

        Celsius boiling = 100;
        Console.WriteLine($"explicit: {boiling} -> {(Fahrenheit)boiling}");
    }
}
