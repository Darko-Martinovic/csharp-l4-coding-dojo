using System.Reflection;

namespace CodingTasks.Core;

/// <summary>
/// Discovers every <see cref="ISolution"/> in the assembly by reflection, the
/// same way <see cref="TaskRegistry"/> finds tasks. Dropping a new file into
/// <c>Solutions/</c> auto-registers it — no central list to maintain.
/// </summary>
public static class SolutionRegistry
{
    public static IReadOnlyList<ISolution> LoadAll()
    {
        var solutions = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ISolution).IsAssignableFrom(t)
                        && t is { IsInterface: false, IsAbstract: false }
                        && t.GetConstructor(Type.EmptyTypes) is not null)
            .Select(t => (ISolution)Activator.CreateInstance(t)!)
            .OrderBy(s => s.Id)
            .ToList();

        var dupes = solutions.GroupBy(s => s.Id).Where(g => g.Count() > 1).ToList();
        if (dupes.Count > 0)
        {
            var ids = string.Join(", ", dupes.Select(g => g.Key));
            throw new InvalidOperationException($"Duplicate solution Id(s): {ids}");
        }

        return solutions;
    }
}
