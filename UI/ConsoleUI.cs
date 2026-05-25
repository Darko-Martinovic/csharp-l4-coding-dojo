using System.Text;
using CodingTasks.Core;

namespace CodingTasks.UI;

/// <summary>
/// All console rendering lives here: banners, the grouped task menu, the task
/// detail screen and the live-run output frame. Pure presentation — it never
/// knows what a task <em>does</em>, only how to draw it.
/// </summary>
public static class ConsoleUI
{
    private const int Width = 78;

    // Box-drawing glyphs (UTF-8). Console encoding is forced in Init().
    private const char H = '─', V = '│';
    private const string TL = "┌", TR = "┐", BL = "└", BR = "┘";

    public static void Init()
    {
        Console.OutputEncoding = Encoding.UTF8;
        try { Console.Title = "C# L4 Coding Dojo"; } catch { /* not a real console */ }
    }

    // Console.Clear() throws when output is redirected (e.g. piped / no TTY).
    private static void SafeClear()
    {
        try { Console.Clear(); } catch (IOException) { Console.WriteLine(); }
    }

    // ── Colour helpers ───────────────────────────────────────────────────
    private static void With(ConsoleColor c, Action body)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = c;
        try { body(); } finally { Console.ForegroundColor = prev; }
    }

    private static void Write(ConsoleColor c, string s) => With(c, () => Console.Write(s));
    private static void Line(ConsoleColor c, string s) => With(c, () => Console.WriteLine(s));

    // ── Primitive frames ─────────────────────────────────────────────────
    private static string Top(string title = "")
    {
        if (title.Length == 0) return TL + new string(H, Width - 2) + TR;
        var label = $" {title} ";
        var fill = Width - 2 - label.Length;
        var left = 2;
        return TL + new string(H, left) + label + new string(H, Math.Max(0, fill - left)) + TR;
    }

    private static string Bottom() => BL + new string(H, Width - 2) + BR;

    private static string Row(string content)
    {
        if (content.Length > Width - 4) content = content[..(Width - 7)] + "...";
        return $"{V} {content.PadRight(Width - 4)} {V}";
    }

    // ── Public building blocks ───────────────────────────────────────────
    public static void Banner()
    {
        SafeClear();
        Line(ConsoleColor.Cyan, Top());
        Line(ConsoleColor.Cyan, Row(""));
        With(ConsoleColor.Cyan, () => Console.Write(V + " "));
        With(ConsoleColor.White, () => Console.Write(Center("C #   L 4   C O D I N G   D O J O", Width - 4)));
        Line(ConsoleColor.Cyan, " " + V);
        With(ConsoleColor.Cyan, () => Console.Write(V + " "));
        With(ConsoleColor.DarkGray, () => Console.Write(Center("30 senior-level challenges · ~5 minutes each", Width - 4)));
        Line(ConsoleColor.Cyan, " " + V);
        Line(ConsoleColor.Cyan, Row(""));
        Line(ConsoleColor.Cyan, Bottom());
    }

    public static void MainMenu(IReadOnlyList<ICodingTask> tasks)
    {
        Banner();
        Console.WriteLine();
        foreach (var group in tasks.GroupBy(t => t.Category))
        {
            Line(ConsoleColor.Yellow, $"  {group.Key}");
            Line(ConsoleColor.DarkGray, "  " + new string(H, Width - 4));
            foreach (var t in group)
            {
                Write(ConsoleColor.Green, $"   {t.Id,2}. ");
                Console.WriteLine(t.Title);
            }
            Console.WriteLine();
        }
        Line(ConsoleColor.DarkGray, "  " + new string(H, Width - 4));
        With(ConsoleColor.White, () => Console.Write("  Enter a task number (1-" + tasks.Count + "), "));
        With(ConsoleColor.White, () => Console.Write("or "));
        Write(ConsoleColor.Red, "Q");
        Console.WriteLine(" to quit.");
        Write(ConsoleColor.Cyan, "  > ");
    }

    /// <summary>Renders one task's prompt + starter code, then drives the
    /// Solution / Run / Back sub-menu until the user backs out. The runnable
    /// answer comes from the matching <paramref name="solution"/> (may be null
    /// if no Solutions/ file has this task's Id).</summary>
    public static void ShowTask(ICodingTask task, ISolution? solution)
    {
        var revealed = false;
        while (true)
        {
            SafeClear();
            Line(ConsoleColor.Cyan, Top($"Task {task.Id} · {task.Category}"));
            Line(ConsoleColor.Cyan, Row(""));
            With(ConsoleColor.Cyan, () => Console.Write(V + " "));
            With(ConsoleColor.White, () => Console.Write(task.Title.PadRight(Width - 4)));
            Line(ConsoleColor.Cyan, " " + V);
            Line(ConsoleColor.Cyan, Row(""));
            Line(ConsoleColor.Cyan, Bottom());

            Console.WriteLine();
            Line(ConsoleColor.Yellow, "  TASK");
            WrapPrint(task.Description, ConsoleColor.Gray);

            Console.WriteLine();
            CodeBlock("Starter Code", task.StarterCode, ConsoleColor.DarkYellow);

            if (revealed)
            {
                Console.WriteLine();
                Line(ConsoleColor.Yellow, "  SOLUTION — explanation");
                WrapPrint(task.SolutionExplanation, ConsoleColor.Gray);
                Console.WriteLine();
                CodeBlock("Solution Code", task.SolutionCode, ConsoleColor.Green);
            }

            Console.WriteLine();
            Line(ConsoleColor.DarkGray, "  " + new string(H, Width - 4));
            Prompt("S", "show solution");
            Prompt("R", "run solution");
            Prompt("B", "back to menu");
            Console.WriteLine();
            Write(ConsoleColor.Cyan, "  > ");

            var key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.S:
                    revealed = true;
                    break;
                case ConsoleKey.R:
                    RunAndShow(solution);
                    break;
                case ConsoleKey.B:
                case ConsoleKey.Escape:
                case ConsoleKey.Backspace:
                    return;
            }
        }
    }

    private static void RunAndShow(ISolution? solution)
    {
        Console.WriteLine();
        CodeBlockOpen("Live Output", ConsoleColor.Magenta);

        if (solution is null)
        {
            Line(ConsoleColor.Gray, "  " + V + " (no runnable solution found for this task)");
            Line(ConsoleColor.Magenta, "  " + Bottom());
            Console.WriteLine();
            Line(ConsoleColor.DarkGray, "  Press any key to return to the task…");
            Console.ReadKey(intercept: true);
            return;
        }

        var original = Console.Out;
        var buffer = new StringWriter();
        string captured;
        Exception? error = null;
        try
        {
            Console.SetOut(buffer);
            solution.Run();
        }
        catch (Exception ex)
        {
            error = ex;
        }
        finally
        {
            Console.SetOut(original);
            captured = buffer.ToString();
        }

        foreach (var l in captured.Replace("\r\n", "\n").Split('\n'))
            Line(ConsoleColor.Gray, "  " + V + " " + l);
        if (error is not null)
            Line(ConsoleColor.Red, "  " + V + " [threw] " + error.GetType().Name + ": " + error.Message);
        Line(ConsoleColor.Magenta, "  " + Bottom());

        Console.WriteLine();
        Line(ConsoleColor.DarkGray, "  Press any key to return to the task…");
        Console.ReadKey(intercept: true);
    }

    // ── Lower-level rendering ────────────────────────────────────────────
    private static void Prompt(string key, string desc)
    {
        Console.Write("  ");
        Write(ConsoleColor.White, "[");
        Write(ConsoleColor.Green, key);
        Write(ConsoleColor.White, "] ");
        Write(ConsoleColor.DarkGray, desc + "    ");
    }

    private static void CodeBlock(string title, string code, ConsoleColor color)
    {
        CodeBlockOpen(title, color);
        foreach (var raw in code.Replace("\r\n", "\n").TrimEnd('\n').Split('\n'))
        {
            Write(color, "  " + V + " ");
            Console.WriteLine(raw);
        }
        Line(color, "  " + Bottom());
    }

    private static void CodeBlockOpen(string title, ConsoleColor color)
        => Line(color, "  " + Top(title));

    private static void WrapPrint(string text, ConsoleColor color)
    {
        foreach (var paragraph in text.Replace("\r\n", "\n").Split('\n'))
        {
            if (paragraph.Trim().Length == 0) { Console.WriteLine(); continue; }
            var words = paragraph.Split(' ');
            var sb = new StringBuilder("  ");
            foreach (var w in words)
            {
                if (sb.Length + w.Length + 1 > Width)
                {
                    Line(color, sb.ToString());
                    sb = new StringBuilder("  ");
                }
                sb.Append(w).Append(' ');
            }
            if (sb.ToString().Trim().Length > 0) Line(color, sb.ToString());
        }
    }

    private static string Center(string s, int width)
    {
        if (s.Length >= width) return s;
        var pad = (width - s.Length) / 2;
        return new string(' ', pad) + s + new string(' ', width - s.Length - pad);
    }

    public static void Goodbye()
    {
        Console.WriteLine();
        Line(ConsoleColor.Cyan, "  Keep shipping. 👋");
        Console.WriteLine();
    }
}
