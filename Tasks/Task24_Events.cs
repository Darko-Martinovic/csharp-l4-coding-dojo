using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task24_Events : ICodingTask
{
    public int Id => 24;
    public string Title => "Use the event pattern, raise it safely";
    public string Category => "Type Members";

    public string Description => """
        Counter exposes a public Action<int> field for change notifications.
        Anyone can overwrite all subscribers (= assignment), or even invoke it
        from outside, and the raise site `if (h != null) h(...)` has a
        null-race when the last handler unsubscribes between the check and the
        call.

        Convert it to a proper event and raise it safely.
        """;

    public string StarterCode => """
        using System;

        class Counter {
            public Action<int> Changed;          // assignable & callable externally
            private int _value;
            public void Increment() {
                _value++;
                if (Changed != null) Changed(_value);   // race between check & call
            }
        }

        public class Program {
            public static void Main() {
                var c = new Counter();
                c.Changed += v => Console.WriteLine($"now {v}");
                c.Increment();   // now 1
                c.Increment();   // now 2
            }
        }
        """;

    public string SolutionExplanation => """
        The `event` keyword restricts outside code to += and -= only — it can't
        reassign or invoke the delegate. Use the standard EventHandler<T> shape
        (sender, args) for consistency with the framework. Raise with
        `Changed?.Invoke(this, e)`: the null-conditional reads the delegate into
        a hidden temporary once and calls it only if non-null, which removes the
        check-then-call race (delegates are immutable, so the snapshot is safe).
        """;

    public string SolutionCode => """
        class ValueChangedEventArgs(int value) : EventArgs {
            public int Value { get; } = value;
        }

        class Counter {
            public event EventHandler<ValueChangedEventArgs>? Changed;  // += / -= only
            private int _value;
            public void Increment() {
                _value++;
                Changed?.Invoke(this, new ValueChangedEventArgs(_value)); // race-free
            }
        }

        var c = new Counter();
        c.Changed += (sender, e) => Console.WriteLine($"now {e.Value}");
        c.Increment();   // prints "now 1"
        """;
}
