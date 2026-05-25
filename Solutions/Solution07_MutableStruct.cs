using CodingTasks.Core;

namespace CodingTasks.Solutions.S07;

// Immutable value type: Increment returns a new value instead of mutating in
// place, so the copy semantics of `list[0]` become explicit and bug-resistant.
public readonly struct Counter(int value)
{
    public int Value { get; } = value;
    public Counter Increment() => new(Value + 1);
}

public sealed class Solution07_MutableStruct : ISolution
{
    public int Id => 7;

    public void Run()
    {
        var list = new List<Counter> { new(0) };
        Console.WriteLine($"start: {list[0].Value}");
        list[0] = list[0].Increment();   // copy out, transform, store back
        list[0] = list[0].Increment();
        Console.WriteLine($"after two increments: {list[0].Value}");   // 2
    }
}
