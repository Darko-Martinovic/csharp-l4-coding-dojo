using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task14_PatternMatching : ICodingTask
{
    public int Id => 14;
    public string Title => "Replace if/else chains with switch expressions";
    public string Category => "Records & Patterns";

    public string Description => """
        Two methods are written as verbose if/else ladders: one classifies a
        temperature with overlapping range checks, the other computes a shape's
        area with is-checks and casts.

        Rewrite both as switch expressions using relational, logical, type and
        property patterns. The exhaustiveness should be obvious.
        """;

    public string StarterCode => """
        string Describe(int t) {
            if (t < 0) return "freezing";
            else if (t >= 0 && t < 15) return "cold";
            else if (t >= 15 && t < 25) return "mild";
            else return "hot";
        }

        double Area(object shape) {
            if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
            if (shape is Rectangle r) return r.W * r.H;
            throw new ArgumentException("unknown shape");
        }
        """;

    public string SolutionExplanation => """
        Switch expressions evaluate patterns top-to-bottom and return a value,
        so adjacent ranges don't need redundant lower bounds — once `< 0` is
        ruled out, `< 15` means [0,15). Relational (`<`) and logical (`and`,
        `or`) patterns express ranges directly. For shapes, type patterns with
        property sub-patterns (`Circle { Radius: var r }`) bind and test in one
        step, and a `null`/`_` arm makes the unhandled cases explicit.
        """;

    public string SolutionCode => """
        string Describe(int t) => t switch {
            < 0       => "freezing",
            < 15      => "cold",      // i.e. [0, 15)
            < 25      => "mild",      // i.e. [15, 25)
            _         => "hot",
        };

        double Area(object shape) => shape switch {
            Circle    { Radius: var r }    => Math.PI * r * r,
            Rectangle { W: var w, H: var h } => w * h,
            null => throw new ArgumentNullException(nameof(shape)),
            _    => throw new ArgumentException("unknown shape"),
        };
        """;
}
