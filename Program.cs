using CodingTasks.Core;
using CodingTasks.UI;

ConsoleUI.Init();

var tasks = TaskRegistry.LoadAll();
var byId = tasks.ToDictionary(t => t.Id);

var solutions = SolutionRegistry.LoadAll();
var solById = solutions.ToDictionary(s => s.Id);

// Hidden smoke-test mode: `dotnet run -- selftest` runs every Solutions/ file
// (the only runnable code) and reports any that throw, plus any task lacking a
// matching solution. Handy for CI and for non-interactive verification.
if (args.Length > 0 && args[0].Equals("selftest", StringComparison.OrdinalIgnoreCase))
{
    var failures = 0;

    // Every task should have a Solutions/ file with the same Id.
    foreach (var t in tasks.Where(t => !solById.ContainsKey(t.Id)))
    {
        failures++;
        Console.WriteLine($"  MISSING solution for task {t.Id,2}. {t.Title}");
    }

    Console.WriteLine("Solutions (Solutions/ files):");
    foreach (var s in solutions)
    {
        var title = byId.TryGetValue(s.Id, out var mt) ? mt.Title : "(no matching task)";
        var saved = Console.Out;
        using var sink = new StringWriter();
        Console.SetOut(sink);
        try
        {
            s.Run();
            Console.SetOut(saved);
            Console.WriteLine($"  OK   {s.Id,2}. {title}");
        }
        catch (Exception ex)
        {
            Console.SetOut(saved);
            failures++;
            Console.WriteLine($"  FAIL {s.Id,2}. {title} -> {ex.GetType().Name}: {ex.Message}");
        }
    }

    Console.WriteLine($"\n{tasks.Count} task(s), {solutions.Count} solution(s), {failures} failure(s).");
    return failures == 0 ? 0 : 1;
}

while (true)
{
    ConsoleUI.MainMenu(tasks);

    var input = Console.ReadLine()?.Trim();
    if (input is null) break;                                   // piped EOF
    if (input.Length == 0) continue;
    if (input.Equals("q", StringComparison.OrdinalIgnoreCase)) break;

    if (int.TryParse(input, out var id) && byId.TryGetValue(id, out var task))
        ConsoleUI.ShowTask(task, solById.GetValueOrDefault(id));
    // anything else: silently redraw the menu
}

ConsoleUI.Goodbye();
return 0;
