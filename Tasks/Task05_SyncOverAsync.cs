using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task05_SyncOverAsync : ICodingTask
{
    public int Id => 5;
    public string Title => "Async-over-sync: deadlock and swallowed errors";
    public string Category => "Async & Tasks";

    public string Description => """
        This service blocks on async code with .Result and exposes an
        `async void` entry point. Under a captured SynchronizationContext (UI
        / classic ASP.NET) the .Result call deadlocks, and the async void
        method's exceptions can't be caught by the caller.

        Refactor to be correct: async all the way down, awaitable, and with
        observable exceptions.
        """;

    public string StarterCode => """
        using System;
        using System.Threading.Tasks;

        public class Service {
            public async void Run() {                 // bug: async void
                var data = GetAsync().Result;         // bug: blocks -> deadlock risk
                Console.WriteLine(data.Length);
            }

            private async Task<string> GetAsync() {
                await Task.Delay(10);
                return "payload";
            }
        }
        """;

    public string SolutionExplanation => """
        Two problems:
        • .Result (and .Wait()) synchronously block the calling thread while the
          awaited continuation tries to resume on that same captured context →
          classic deadlock. Replace with `await`.
        • `async void` cannot be awaited and its exceptions escape to the
          synchronization context (often crashing the process). Return `Task`
          so callers can await and observe failures.

        Make the whole chain async; only top-level event handlers should ever
        be async void.
        """;

    public string SolutionCode => """
        public class Service {
            public async Task RunAsync() {            // returns Task, awaitable
                var data = await GetAsync();          // no blocking
                Console.WriteLine(data.Length);
            }

            private async Task<string> GetAsync() {
                await Task.Delay(10);
                return "payload";
            }
        }

        // caller: await service.RunAsync();
        """;
}
