using CodingTasks.Core;

namespace CodingTasks.Solutions.S08;

public sealed class Solution08_BoxingEquality : ISolution
{
    public int Id => 8;

    public void Run()
    {
        object a = 5;
        object b = 5;

        // Boxing allocates two distinct objects, so `==` (reference equality on
        // object) is false; Equals dispatches to Int32.Equals and compares values.
        Console.WriteLine($"ReferenceEquals(a, b)   = {ReferenceEquals(a, b)}");
        Console.WriteLine($"a == b  (object ==)     = {a == b}");
        Console.WriteLine($"a.Equals(b)             = {a.Equals(b)}");
        Console.WriteLine($"EqualityComparer.Equals = {EqualityComparer<object>.Default.Equals(a, b)}");
        Console.WriteLine($"(int)a == (int)b        = {(int)a == (int)b}");
    }
}
