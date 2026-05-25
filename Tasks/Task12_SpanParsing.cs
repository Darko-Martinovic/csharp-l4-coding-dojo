using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task12_SpanParsing : ICodingTask
{
    public int Id => 12;
    public string Title => "Sum a CSV string without heap allocations";
    public string Category => "Memory & Performance";

    public string Description => """
        SumCsv parses a comma-separated list of integers. The current version
        calls string.Split, which allocates a string[] plus one substring per
        token — wasteful on a hot path.

        Rewrite it to allocate nothing on the heap using ReadOnlySpan<char>
        slicing and span-based int.Parse.
        """;

    public string StarterCode => """
        using System;

        int SumCsv(string csv) {
            var parts = csv.Split(',');     // allocates array + N substrings
            int sum = 0;
            foreach (var p in parts)
                sum += int.Parse(p);
            return sum;
        }
        """;

    public string SolutionExplanation => """
        Slicing a ReadOnlySpan<char> creates no new objects — a slice is just a
        (pointer, length) view over the original characters. Walk the span,
        find each comma with IndexOf, parse the leading slice with the
        span overload of int.Parse (no substring), then re-slice past the comma.
        Zero allocations, and the parsing is identical in result to Split.
        """;

    public string SolutionCode => """
        int SumCsv(ReadOnlySpan<char> csv) {
            int sum = 0;
            while (true) {
                int comma = csv.IndexOf(',');
                ReadOnlySpan<char> token = comma < 0 ? csv : csv[..comma];
                sum += int.Parse(token);             // span overload, no alloc
                if (comma < 0) break;
                csv = csv[(comma + 1)..];            // slice, not substring
            }
            return sum;
        }
        // call: SumCsv("1,22,333,4")  ->  360
        """;
}
