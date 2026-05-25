using CodingTasks.Core;

namespace CodingTasks.Solutions.S15;

public sealed class Solution15_NullableReferences : ISolution
{
    public int Id => 15;

    private readonly Dictionary<int, string> _names = new() { [1] = "Ada" };

    // The return type tells the truth: it may be null, so callers must handle it.
    private string? FindName(int id) => _names.GetValueOrDefault(id);

    public void Run()
    {
        var missing = FindName(99);
        Console.WriteLine($"FindName(99) -> {missing?.ToUpper() ?? "<unknown>"}");

        if (FindName(1) is { } found)
            Console.WriteLine($"FindName(1)  -> {found.ToUpper()}");
    }
}
