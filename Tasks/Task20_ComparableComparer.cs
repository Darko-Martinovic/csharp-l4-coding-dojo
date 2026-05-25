using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task20_ComparableComparer : ICodingTask
{
    public int Id => 20;
    public string Title => "Sorting a custom type (IComparable vs IComparer)";
    public string Category => "Comparison & Sorting";

    public string Description => """
        people.Sort() throws InvalidOperationException: "Failed to compare two
        elements" because Person defines no ordering. Give Person a natural
        order, and additionally provide a way to sort by a different key
        on demand — without modifying the type each time.
        """;

    public string StarterCode => """
        using System;
        using System.Collections.Generic;

        class Person {
            public string Name = "";
            public int Age;
        }

        public class Program {
            public static void Main() {
                var people = new List<Person> {
                    new Person { Name = "Alice", Age = 30 },
                    new Person { Name = "Bob", Age = 40 },
                };
                people.Sort();   // InvalidOperationException: no IComparable
            }
        }
        """;

    public string SolutionExplanation => """
        Two complementary tools:
        • IComparable<T>.CompareTo defines the type's *natural* order (used by
          Sort(), Array.Sort, SortedSet). Return <0 / 0 / >0; delegate to the
          framework's Comparer for the key (e.g. Age.CompareTo(other.Age)).
        • IComparer<T> is an *external*, swappable ordering you pass to Sort or
          OrderBy when you need a different key (by Name, descending, etc.)
          without touching the type. Comparer<T>.Create(...) builds one inline.
        """;

    public string SolutionCode => """
        class Person : IComparable<Person> {
            public string Name = "";
            public int Age;
            public int CompareTo(Person? other)        // natural order: by Age
                => Age.CompareTo(other?.Age ?? int.MinValue);
        }

        people.Sort();                                  // by Age (natural)

        var byName = Comparer<Person>.Create((a, b) =>
            string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        people.Sort(byName);                            // by Name (external)
        """;
}
