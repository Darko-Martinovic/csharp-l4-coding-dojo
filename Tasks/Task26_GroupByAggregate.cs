using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task26_GroupByAggregate : ICodingTask
{
    public int Id => 26;
    public string Title => "Per-category totals with LINQ GroupBy";
    public string Category => "Generics & LINQ";

    public string Description => """
        The code computes the total amount per category using a manual
        dictionary and nested mutation. Rewrite it as a single declarative LINQ
        query that groups by category, sums each group, and returns the results
        ordered by total descending.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        record Order(string Category, decimal Amount);

        public class Program {
            public static void Main() {
                var orders = new List<Order> {
                    new("Books", 12m), new("Toys", 30m), new("Books", 8m),
                };

                var totals = new Dictionary<string, decimal>();
                foreach (var o in orders) {
                    if (!totals.ContainsKey(o.Category))
                        totals[o.Category] = 0;
                    totals[o.Category] += o.Amount;
                }
                // ...then sort the dictionary by value somehow...
                foreach (var t in totals)
                    Console.WriteLine($"{t.Key}: {t.Value}");
            }
        }
        """;

    public string SolutionExplanation => """
        GroupBy partitions the sequence by a key selector and hands each group
        as an IGrouping<TKey, T> (itself enumerable). Project each group to an
        anonymous/record result with Sum over its items, then OrderByDescending.
        It reads as the intent ("total per category, biggest first") and is a
        single pass conceptually. For a scalar fold across all items, Aggregate
        (or the specialised Sum/Min/Max/Average) is the tool.
        """;

    public string SolutionCode => """
        var totals = orders
            .GroupBy(o => o.Category)
            .Select(g => new { Category = g.Key, Total = g.Sum(o => o.Amount) })
            .OrderByDescending(x => x.Total);

        decimal grand = orders.Sum(o => o.Amount);     // or .Aggregate(0m, (a, o) => a + o.Amount)
        """;
}
