using CodingTasks.Core;

namespace CodingTasks.Solutions.S04;

public sealed class Solution04_ClosureCapture : ISolution
{
    public int Id => 4;

    public void Run()
    {
        // A fresh variable per iteration gives each closure its own captured
        // cell, so they print the index they were created with.
        var actions = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            int copy = i;
            actions.Add(() => copy);
        }

        Console.WriteLine(string.Join(" ", actions.Select(a => a())));   // 0 1 2
    }
}
