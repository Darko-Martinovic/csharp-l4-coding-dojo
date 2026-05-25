using CodingTasks.Core;

namespace CodingTasks.Solutions.S03;

// The deliverable: a generic join extension. The second overload does the work;
// the first delegates to it with a ToString() selector, so there is no
// duplicated join logic. Arguments are validated eagerly.
public static class EnumerableStringExtensions
{
    public static string AsString<T>(this IEnumerable<T> items, string separator)
        => items.AsString(x => x?.ToString() ?? string.Empty, separator);

    public static string AsString<T>(
        this IEnumerable<T> items,
        Func<T, string> selector,
        string separator)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(selector);
        return string.Join(separator, items.Select(selector));
    }
}

public sealed class Solution03_AsStringExtension : ISolution
{
    public int Id => 3;

    private enum Status { New, InProgress, Done }
    private sealed record Person(string Name, int Age);

    public void Run()
    {
        var ints = new List<int> { 1, 2, 3, 4, 5 };
        var statuses = new List<Status> { Status.New, Status.InProgress, Status.Done };
        var people = new List<Person>
        {
            new("Alice", 30), new("Bob", 40), new("Charlie", 35)
        };

        Console.WriteLine("Integers: " + ints.AsString(", "));
        Console.WriteLine("Enum:     " + statuses.AsString(s => $"[{s}]", " | "));
        Console.WriteLine("Objects:  " + people.AsString(p => $"{p.Name} ({p.Age})", " / "));
    }
}
