using CodingTasks.Core;

namespace CodingTasks.Solutions.S24;

public sealed class ValueChangedEventArgs(int value) : EventArgs
{
    public int Value { get; } = value;
}

// `event` restricts outsiders to += / -= (no reassign or invoke). The
// null-conditional raise snapshots the delegate once, removing the check-call race.
public sealed class Counter
{
    public event EventHandler<ValueChangedEventArgs>? Changed;
    private int _value;
    public void Increment()
    {
        _value++;
        Changed?.Invoke(this, new ValueChangedEventArgs(_value));
    }
}

public sealed class Solution24_Events : ISolution
{
    public int Id => 24;

    public void Run()
    {
        var c = new Counter();
        c.Changed += (_, e) => Console.WriteLine($"handler A: value is now {e.Value}");
        c.Changed += (_, e) => Console.WriteLine($"handler B: value is now {e.Value}");
        c.Increment();
        c.Increment();
    }
}
