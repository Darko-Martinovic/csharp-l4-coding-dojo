using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task28_DefaultInterfaceMethods : ICodingTask
{
    public int Id => 28;
    public string Title => "Extend an interface without breaking implementers";
    public string Category => "Type Members";

    public string Description => """
        ILogger has one method, Log, and several existing implementers. You need
        to add a LogError convenience method to the interface — but adding a
        normal member would force every implementer to be edited or they stop
        compiling. Add it without touching any existing implementation.
        """;

    public string StarterCode => """
        using System;

        interface ILogger {
            void Log(string message);
            void LogError(string message);   // <-- adding this breaks every
        }                                    //     class that already implements ILogger

        class ConsoleLogger : ILogger {
            public void Log(string m) => Console.WriteLine(m);
            // CS0535: does not implement 'ILogger.LogError'
        }
        """;

    public string SolutionExplanation => """
        Default interface methods (C# 8+) let an interface ship a method *body*.
        Implementers that don't override it inherit the default, so adding
        LogError is non-breaking. Note the dispatch rule: a default method is
        callable through the *interface* type, not through the concrete class
        reference — so call it via an ILogger variable (or have a class override
        it to surface it on the instance). Great for evolving published APIs.
        """;

    public string SolutionCode => """
        interface ILogger {
            void Log(string message);
            void LogError(string message)            // default implementation
                => Log($"ERROR: {message}");
        }

        class ConsoleLogger : ILogger {
            public void Log(string m) => Console.WriteLine(m);
            // no LogError needed — inherits the default
        }

        ILogger logger = new ConsoleLogger();
        logger.LogError("disk full");   // ERROR: disk full  (via interface ref)
        """;
}
