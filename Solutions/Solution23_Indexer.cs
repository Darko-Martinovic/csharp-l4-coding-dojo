using CodingTasks.Core;

namespace CodingTasks.Solutions.S23;

// An indexer (`this[...]`) replaces Get/Set methods. Backing the dictionary with
// OrdinalIgnoreCase makes lookups case-insensitive; the getter returns "" rather
// than throwing for an absent header.
public sealed class HeaderBag
{
    private readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase);
    public string this[string name]
    {
        get => _map.GetValueOrDefault(name, "");
        set => _map[name] = value;
    }
}

public sealed class Solution23_Indexer : ISolution
{
    public int Id => 23;

    public void Run()
    {
        var bag = new HeaderBag();
        bag["Content-Type"] = "text/plain";
        Console.WriteLine($"bag[\"content-type\"] = {bag["content-type"]}");
        Console.WriteLine($"bag[\"missing\"]      = [{bag["missing"]}]");
    }
}
