# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A single-project .NET 10 console app ("C# L4 Coding Dojo") presenting 30 senior-level C# challenges. Each challenge is split in two: a **task** (pure metadata — prompt, starter snippet, and a worked answer shown as text) and a matching **solution** (the only runnable code, in its own file).

## Commands

```bash
dotnet run                 # interactive menu (pick task 1-30; S=solution, R=run, B=back, Q=quit)
dotnet run -- selftest     # run every Solutions/ file + check each task has a matching solution; exit 1 on failure
dotnet build               # compile
```

There is **no separate test project**. `selftest` is the test/CI mechanism — it runs every standalone `Solutions/` file and reports any that throw, plus any task lacking a matching solution. Run it after touching any solution. To watch one solution's output, run interactively and press `R` on its task.

## Architecture

- **`Core/ICodingTask.cs`** — the metadata contract every task implements: `Id`, `Title`, `Category`, `Description`, `StarterCode`, `SolutionExplanation`, `SolutionCode`. **All are display strings; a task holds no runnable code.**
- **`Core/ISolution.cs`** — the contract for the runnable reference solutions: `Id` (matches the task's `Id`) + `void Run()`.
- **`Core/TaskRegistry.cs`** / **`Core/SolutionRegistry.cs`** — `LoadAll()` reflects over the assembly and instantiates every non-abstract `ICodingTask` / `ISolution` with a parameterless constructor, ordered by `Id`. **There is no central registration list** — dropping a file in `Tasks/` or `Solutions/` auto-registers it. Duplicate `Id`s throw at startup.
- **`UI/ConsoleUI.cs`** — pure presentation (banner, grouped menu, task screen, live-output frame). `ShowTask(task, solution)` draws the task; pressing `R` runs the paired `ISolution` (output captured via a `StringWriter`).
- **`Program.cs`** — entry point: loads tasks + solutions, pairs them by `Id`, handles `selftest`, then runs the menu loop.
- **`Tasks/TaskNN_*.cs`** — one challenge's metadata per file (strings only).
- **`Solutions/SolutionNN_*.cs`** — the real, compiled solution for task `NN`, each in its own `CodingTasks.Solutions.SNN` namespace so the deliverable types can sit at top level without colliding across files.

## The key convention: task and solution are separate, paired by Id

A challenge lives in **two files**, linked only by their shared `Id`:

1. `Tasks/TaskNN_*.cs` — metadata. `StarterCode` is the broken/incomplete snippet; `SolutionCode` is the worked answer **as a display string**. Both may contain intentionally non-compiling or buggy C# — they are never compiled. (`TreatWarningsAsErrors` is off for this reason; the runnable solution code must stay warning-clean.)
2. `Solutions/SolutionNN_*.cs` — the actual compiled answer: an `ISolution` whose `Run()` shows **only the correct way** to solve it (no "before/buggy" variant) and writes its output to the console.

`Run()` must be **synchronous**: async demos bridge at the top level via `.GetAwaiter().GetResult()` (safe here — a console app has no captured `SynchronizationContext`). Only implicit usings (`System`, `System.Linq`, `System.Threading[.Tasks]`, …) are available; add explicit `using`s for anything else (e.g. `System.Diagnostics`, `System.Linq.Expressions`).

To add a challenge: add `Tasks/TaskNN_*.cs` (unique `Id`, the seven string/int members) and `Solutions/SolutionNN_*.cs` (matching `Id`, its own `CodingTasks.Solutions.SNN` namespace), then confirm with `dotnet run -- selftest`.
