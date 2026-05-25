using CodingTasks.Core;

namespace CodingTasks.Solutions.S25;

public sealed class Solution25_GenericConstraints : ISolution
{
    public int Id => 25;

    // `where T : IComparable<T>` lets the compiler allow CompareTo, so Max works
    // for any orderable type (int, string, DateTime, custom IComparable<T>).
    private static T Max<T>(T a, T b) where T : IComparable<T>
        => a.CompareTo(b) >= 0 ? a : b;

    public void Run()
    {
        Console.WriteLine($"Max(3, 7)               = {Max(3, 7)}");
        Console.WriteLine($"Max(\"apple\", \"banana\") = {Max("apple", "banana")}");
        Console.WriteLine($"Max(2.71, 3.14)         = {Max(2.71, 3.14)}");
    }
}
