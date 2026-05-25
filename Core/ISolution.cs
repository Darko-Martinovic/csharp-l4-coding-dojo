namespace CodingTasks.Core;

/// <summary>
/// A real, compiled reference solution living in its own file under
/// <c>Solutions/</c>. Where <see cref="ICodingTask.SolutionCode"/> is a
/// display-only string, this is the actual code: it compiles with the project
/// and runs under <c>dotnet run -- selftest</c>. The <see cref="Id"/> matches
/// the corresponding task's Id.
/// </summary>
public interface ISolution
{
    /// <summary>Matches the corresponding <see cref="ICodingTask.Id"/>.</summary>
    int Id { get; }

    /// <summary>Executes the solution and writes its output to the console.</summary>
    void Run();
}
