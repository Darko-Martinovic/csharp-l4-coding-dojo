using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task04_ClosureCapture : ICodingTask
{
    public int Id => 4;
    public string Title => "Captured loop variable prints the wrong values";
    public string Category => "Closures & Captures";

    public string Description => """
        The code builds three callbacks inside a for-loop, each meant to print
        its own index (expected: 0 1 2). Instead it prints 3 3 3.

        Explain why, then fix it so each callback prints the index it was
        created with — without changing the loop type.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        var actions = new List<Action>();
        for (int i = 0; i < 3; i++)
            actions.Add(() => Console.Write(i + " "));

        foreach (var a in actions) a();   // prints "3 3 3"
        """;

    public string SolutionExplanation => """
        A lambda captures the *variable*, not its value at capture time. In a
        for-loop there is exactly one `i` shared by every iteration, so all
        three closures reference the same storage — which equals 3 once the
        loop exits.

        Fix: introduce a fresh variable inside the loop body. Each iteration
        gets its own `copy`, so each closure captures a distinct cell.
        (A C# 5+ foreach over a range works too, because foreach declares a new
        iteration variable per pass — but a for-loop does not.)
        """;

    public string SolutionCode => """
        var actions = new List<Action>();
        for (int i = 0; i < 3; i++)
        {
            int copy = i;                 // new variable each iteration
            actions.Add(() => Console.Write(copy + " "));
        }

        foreach (var a in actions) a();   // prints "0 1 2"
        """;
}
