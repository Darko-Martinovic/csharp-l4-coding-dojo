using CodingTasks.Core;

namespace CodingTasks.Solutions.S12;

public sealed class Solution12_SpanParsing : ISolution
{
    public int Id => 12;

    // Slicing a ReadOnlySpan<char> allocates nothing — each slice is a
    // (pointer, length) view. Find each comma, parse the leading slice with the
    // span overload of int.Parse, then re-slice past the comma.
    private static int SumCsv(ReadOnlySpan<char> csv)
    {
        int sum = 0;
        while (true)
        {
            int comma = csv.IndexOf(',');
            ReadOnlySpan<char> token = comma < 0 ? csv : csv[..comma];
            sum += int.Parse(token);
            if (comma < 0) break;
            csv = csv[(comma + 1)..];
        }
        return sum;
    }

    public void Run()
    {
        const string input = "1,22,333,4";
        Console.WriteLine($"SumCsv(\"{input}\") = {SumCsv(input)}");   // 360
        Console.WriteLine($"SumCsv(\"42\")          = {SumCsv("42")}");
        Console.WriteLine("No string[] or substrings were allocated.");
    }
}
