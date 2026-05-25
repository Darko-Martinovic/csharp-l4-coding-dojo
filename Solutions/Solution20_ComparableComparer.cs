using CodingTasks.Core;

namespace CodingTasks.Solutions.S20;

// IComparable<T> defines the type's natural order (used by Sort()); an external
// IComparer<T> supplies a different, swappable order on demand.
public sealed class Person : IComparable<Person>
{
    public string Name = "";
    public int Age;
    public int CompareTo(Person? other) => Age.CompareTo(other?.Age ?? int.MinValue);
    public override string ToString() => $"{Name}({Age})";
}

public sealed class Solution20_ComparableComparer : ISolution
{
    public int Id => 20;

    public void Run()
    {
        List<Person> People() =>
        [
            new() { Name = "Charlie", Age = 35 },
            new() { Name = "Alice", Age = 30 },
            new() { Name = "Bob", Age = 40 },
        ];

        var byAge = People();
        byAge.Sort();
        Console.WriteLine("natural (by Age):  " + string.Join(", ", byAge));

        var byName = People();
        byName.Sort(Comparer<Person>.Create((a, b) =>
            string.Compare(a.Name, b.Name, StringComparison.Ordinal)));
        Console.WriteLine("external (by Name): " + string.Join(", ", byName));
    }
}
