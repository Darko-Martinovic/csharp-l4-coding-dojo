using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task03_AsStringExtension : ICodingTask
{
    public int Id => 3;
    public string Title => "Generic AsString<T> extension with projection";
    public string Category => "Generics & LINQ";

    public string Description => """
        Create an extension method AsString<T> for IEnumerable<T> that joins a
        collection into one string using a separator.

        • AsString<T>(this IEnumerable<T>, string separator) — uses ToString().
        • AsString<T>(this IEnumerable<T>, Func<T,string> selector, string
          separator) — applies the selector before joining.
        • Add null checks for the source and the selector.

        Expected output:
          Integers: 1, 2, 3, 4, 5
          Enum:     [New] | [InProgress] | [Done]
          Objects:  Alice (30) / Bob (40) / Charlie (35)
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        public static class EnumExtensions {
            // TODO: join via item.ToString()
            public static string AsString<T>(this IEnumerable<T> items, string separator)
                => throw new NotImplementedException();

            // TODO: apply selector, then join
            public static string AsString<T>(this IEnumerable<T> items, Func<T, string> selector, string separator)
                => throw new NotImplementedException();
        }

        public class Program {
            public static void Main() {
                var ints = new List<int> { 1, 2, 3, 4, 5 };
                Console.WriteLine(ints.AsString(", "));                 // expected: 1, 2, 3, 4, 5
                Console.WriteLine(ints.AsString(x => $"[{x}]", " | ")); // expected: [1] | [2] | ...
            }
        }
        """;

    public string SolutionExplanation => """
        The second overload does the real work; the first simply delegates to
        it with `x => x?.ToString() ?? string.Empty` as the selector — no
        duplicated join logic. Validate arguments eagerly with
        ArgumentNullException.ThrowIfNull, then use string.Join over a Select
        projection. string.Join itself handles the separator placement.
        """;

    public string SolutionCode => """
        public static class EnumExtensions {
            public static string AsString<T>(this IEnumerable<T> items, string separator)
                => items.AsString(x => x?.ToString() ?? string.Empty, separator);

            public static string AsString<T>(
                this IEnumerable<T> items,
                Func<T, string> selector,
                string separator) {
                ArgumentNullException.ThrowIfNull(items);
                ArgumentNullException.ThrowIfNull(selector);
                return string.Join(separator, items.Select(selector));
            }
        }

        // usage
        ints.AsString(", ");                       // 1, 2, 3, 4, 5
        statuses.AsString(s => $"[{s}]", " | ");   // [New] | [InProgress] | [Done]
        people.AsString(p => $"{p.Name} ({p.Age})", " / ");
        """;
}
