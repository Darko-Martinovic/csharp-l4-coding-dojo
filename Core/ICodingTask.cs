namespace CodingTasks.Core;

/// <summary>
/// A single self-contained coding challenge — pure metadata, no runnable code.
/// A task ships a description, a broken/incomplete <see cref="StarterCode"/>
/// snippet the candidate works from, and a worked <see cref="SolutionCode"/>
/// shown as text. The real, runnable answer lives separately in a matching
/// <c>Solutions/SolutionNN_*.cs</c> file (see <see cref="ISolution"/>) with the
/// same <see cref="Id"/>.
/// </summary>
public interface ICodingTask
{
    /// <summary>Stable 1-based ordering used by the menu.</summary>
    int Id { get; }

    /// <summary>Short headline shown in the list.</summary>
    string Title { get; }

    /// <summary>Topic used to group the menu (e.g. "Generics", "Async").</summary>
    string Category { get; }

    /// <summary>The "TASK:" prompt the candidate reads.</summary>
    string Description { get; }

    /// <summary>The compiling-or-not snippet the candidate starts from.</summary>
    string StarterCode { get; }

    /// <summary>Plain-English explanation of what the fix is and why.</summary>
    string SolutionExplanation { get; }

    /// <summary>The reference answer, formatted for display.</summary>
    string SolutionCode { get; }
}
