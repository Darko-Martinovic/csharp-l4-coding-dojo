using System.Reflection;

namespace CodingTasks.Core;

/// <summary>
/// Discovers every <see cref="ICodingTask"/> in the assembly by reflection so
/// that dropping a new task file into <c>Tasks/</c> auto-registers it — no
/// central list to maintain.
/// </summary>
public static class TaskRegistry
{
    public static IReadOnlyList<ICodingTask> LoadAll()
    {
        var tasks = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ICodingTask).IsAssignableFrom(t)
                        && t is { IsInterface: false, IsAbstract: false }
                        && t.GetConstructor(Type.EmptyTypes) is not null)
            .Select(t => (ICodingTask)Activator.CreateInstance(t)!)
            .OrderBy(t => t.Id)
            .ToList();

        var dupes = tasks.GroupBy(t => t.Id).Where(g => g.Count() > 1).ToList();
        if (dupes.Count > 0)
        {
            var ids = string.Join(", ", dupes.Select(g => g.Key));
            throw new InvalidOperationException($"Duplicate task Id(s): {ids}");
        }

        return tasks;
    }
}
