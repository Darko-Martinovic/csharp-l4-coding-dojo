using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task23_Indexer : ICodingTask
{
    public int Id => 23;
    public string Title => "Expose a collection through an indexer";
    public string Category => "Type Members";

    public string Description => """
        HeaderBag wraps a dictionary but forces callers through Get/Set
        methods, and lookups are case-sensitive. Replace the methods with a
        proper indexer (this[...]) so callers can write bag["X"] = "y", and
        make header names case-insensitive with a sensible default for missing
        keys.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        class HeaderBag {
            private readonly Dictionary<string, string> _map = new();
            public string Get(string name) => _map[name];   // throws if absent
            public void Set(string name, string value) => _map[name] = value;
        }

        var bag = new HeaderBag();
        bag.Set("Content-Type", "text/plain");
        Console.WriteLine(bag.Get("content-type"));   // KeyNotFoundException
        """;

    public string SolutionExplanation => """
        An indexer is a `this[...]` property: a get/set pair that lets the type
        be used with subscript syntax. Back the dictionary with
        StringComparer.OrdinalIgnoreCase so "Content-Type" and "content-type"
        are the same key, and have the getter return "" (or null) instead of
        throwing for an absent header. Indexers can take any parameter type and
        can be overloaded by signature.
        """;

    public string SolutionCode => """
        class HeaderBag {
            private readonly Dictionary<string, string> _map =
                new(StringComparer.OrdinalIgnoreCase);

            public string this[string name] {
                get => _map.GetValueOrDefault(name, "");
                set => _map[name] = value;
            }
        }

        var bag = new HeaderBag();
        bag["Content-Type"] = "text/plain";
        Console.WriteLine(bag["content-type"]);   // text/plain (case-insensitive)
        Console.WriteLine($"[{bag["missing"]}]"); // [] (no throw)
        """;
}
