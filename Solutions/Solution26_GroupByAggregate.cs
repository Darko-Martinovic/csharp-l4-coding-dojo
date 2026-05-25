using CodingTasks.Core;

namespace CodingTasks.Solutions.S26;

public sealed record Order(string Category, decimal Amount);

public sealed class Solution26_GroupByAggregate : ISolution
{
    public int Id => 26;

    public void Run()
    {
        var orders = new List<Order>
        {
            new("Books", 12m), new("Toys", 30m), new("Books", 8m),
            new("Food", 5m), new("Toys", 15m), new("Food", 22m),
        };

        // GroupBy partitions by key; Sum folds each group; OrderByDescending ranks.
        var totals = orders
            .GroupBy(o => o.Category)
            .Select(g => new { Category = g.Key, Total = g.Sum(o => o.Amount) })
            .OrderByDescending(x => x.Total);

        foreach (var t in totals)
            Console.WriteLine($"{t.Category,-6} {t.Total,8:N2}");

        Console.WriteLine($"{"TOTAL",-6} {orders.Sum(o => o.Amount),8:N2}");
    }
}
