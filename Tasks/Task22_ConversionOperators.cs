using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task22_ConversionOperators : ICodingTask
{
    public int Id => 22;
    public string Title => "Implicit vs explicit conversion operators";
    public string Category => "Operators";

    public string Description => """
        A Celsius value type needs conversions: you'd like to assign a plain
        number to it directly, but converting to Fahrenheit should be a
        deliberate step. Implement the right kind of conversion operator for
        each case and justify which is implicit and which is explicit.
        """;

    public string StarterCode => """
        struct Celsius {
            public double Degrees;
            public Celsius(double d) { Degrees = d; }
        }

        Celsius c = 25;                 // want this to just work
        Fahrenheit f = (Fahrenheit)c;   // want an explicit cast here
        """;

    public string SolutionExplanation => """
        Guideline: make a conversion *implicit* only when it never loses
        information and never throws — the caller shouldn't have to think about
        it. Make it *explicit* when it can lose precision, overflow, fail, or
        change meaning, so the cast documents intent at the call site.

        • double -> Celsius: just labels a number; safe and lossless -> implicit.
        • Celsius -> Fahrenheit: a real scale change the reader should see ->
          explicit. (Round-tripping double precision also argues for explicit.)
        """;

    public string SolutionCode => """
        readonly struct Celsius(double degrees) {
            public double Degrees { get; } = degrees;
            public static implicit operator Celsius(double d) => new(d);     // safe
            public static explicit operator Fahrenheit(Celsius c)            // deliberate
                => new(c.Degrees * 9 / 5 + 32);
            public override string ToString() => $"{Degrees}°C";
        }

        readonly struct Fahrenheit(double degrees) {
            public double Degrees { get; } = degrees;
            public override string ToString() => $"{Degrees}°F";
        }

        Celsius c = 25;                  // implicit from double
        Fahrenheit f = (Fahrenheit)c;    // explicit cast: 77°F
        """;
}
