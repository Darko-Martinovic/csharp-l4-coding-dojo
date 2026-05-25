using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task15_NullableReferences : ICodingTask
{
    public int Id => 15;
    public string Title => "Honour nullable reference type annotations";
    public string Category => "Nullability";

    public string Description => """
        FindName claims to return a non-null string but actually returns null
        for missing ids, so the caller's name.ToUpper() throws a
        NullReferenceException. The compiler even warned about it.

        Annotate the API to tell the truth about nullability and fix the call
        site so the warning — and the crash — go away.
        """;

    public string StarterCode => """
        Dictionary<int, string> _names = new() { [1] = "Ada" };

        string FindName(int id)                       // says: never null
            => _names.GetValueOrDefault(id);          // but returns null!

        var name = FindName(99);
        Console.WriteLine(name.ToUpper());            // NullReferenceException
        """;

    public string SolutionExplanation => """
        Nullable reference types are a compile-time contract: `string` promises
        non-null, `string?` admits null. The method can return null, so its
        return type must be `string?`. That turns the unconditional
        `name.ToUpper()` into a warning, forcing the caller to handle the null
        — here with the null-conditional operator and a `??` fallback (or a
        pattern-match / guard). The annotations flow the truth through the
        codebase and the NRE becomes unrepresentable.
        """;

    public string SolutionCode => """
        string? FindName(int id)                      // may be null
            => _names.GetValueOrDefault(id);

        var name = FindName(99);
        Console.WriteLine(name?.ToUpper() ?? "<unknown>");   // safe

        // or branch on it:
        if (FindName(1) is { } found)
            Console.WriteLine(found.ToUpper());
        """;
}
