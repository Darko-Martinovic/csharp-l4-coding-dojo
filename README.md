# C# L4 Coding Dojo

A console app with **30 senior-level (L4) C# challenges**. Each is designed to
be solved in ~5 minutes. Every challenge is split in two:

- a **task** (`Tasks/`) — the prompt, a **starter snippet** (broken or
  incomplete) to work from, and a **worked answer shown as text**
  (explanation + reference code). A task is pure metadata; it holds no
  runnable code.
- a **solution** (`Solutions/`) — the real, compiled answer for that task. The
  app **runs it live** so you can see the expected output.

The two are linked only by a shared task number.

## Run it

```bash
dotnet run            # interactive menu
```

In the menu, type a task number (1–30) and press Enter, then on the task screen:

| Key | Action            |
|-----|-------------------|
| `S` | reveal the solution (explanation + code) |
| `R` | run the solution and show its live output |
| `B` | back to the menu  |

Press `Q` at the menu to quit.

## Self-test (CI-friendly)

Runs every task's live solution **and** every standalone file in `Solutions/`,
reporting any that throw — no interaction needed:

```bash
dotnet run -- selftest
# -> "30 task(s), 30 solution(s), 0 failure(s)."  (exit code 0 on success, 1 on any failure)
```

## Topics covered

Delegates & variance · resource management (IDisposable) · generics &
constraints · closures · async/await (deadlocks, `WhenAll`, `CancellationToken`,
`IAsyncEnumerable`) · iterators & deferred execution · value types & boxing ·
equality (`Equals`/`GetHashCode`) · `Span<T>` · records & pattern matching ·
nullable reference types · concurrency / `Interlocked` · custom exceptions &
filters · `IComparable`/`IComparer` · operator & conversion overloading ·
indexers · events · LINQ `GroupBy`/`Aggregate` · default interface methods ·
expression trees · `ref` returns.

## Project layout

```
Program.cs              Entry point + menu loop + `selftest` mode
Core/
  ICodingTask.cs        The contract every task implements
  ISolution.cs          The contract for the standalone reference solutions
  TaskRegistry.cs       Reflection-based auto-discovery of tasks
  SolutionRegistry.cs   Reflection-based auto-discovery of solutions
UI/
  ConsoleUI.cs          All rendering (banner, menu, task screen, run frame)
Tasks/
  Task01_*.cs … Task30_*.cs       One self-contained challenge per file
Solutions/
  Solution01_*.cs … Solution30_*.cs   The real, compiled solution for each task
```

The displayed `SolutionCode` string lives in `Tasks/`; the corresponding
**real, compilable** solution lives in `Solutions/SolutionNN_*.cs` (same number),
each in its own namespace and runnable via the menu's `R` key or `selftest`.

## Add a new challenge

It takes two files, linked by a shared `Id`:

1. **Task** — drop a file in `Tasks/` implementing `ICodingTask` with a unique
   `Id` (prompt + starter snippet + worked answer as strings, no runnable code).
   It is auto-discovered at startup (no list to edit) and grouped in the menu by
   its `Category`.
2. **Solution** — drop a file in `Solutions/` implementing `ISolution` with the
   **same `Id`**, in its own `CodingTasks.Solutions.SNN` namespace, whose `Run()`
   shows the correct way to solve it.

Run `dotnet run -- selftest` to confirm the solution executes and is paired with
its task.
