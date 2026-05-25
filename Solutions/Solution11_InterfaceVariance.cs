using CodingTasks.Core;

namespace CodingTasks.Solutions.S11;

// A producer only outputs T -> mark it `out` (covariant).
public interface IProducer<out T> { T Produce(); }

// A consumer only accepts T -> mark it `in` (contravariant).
public interface IConsumer<in T> { void Consume(T item); }

public sealed class StringProducer : IProducer<string>
{
    public string Produce() => "hello";
}

public sealed class ObjectConsumer : IConsumer<object>
{
    public void Consume(object item) => Console.WriteLine($"consumed: {item}");
}

public sealed class Solution11_InterfaceVariance : ISolution
{
    public int Id => 11;

    public void Run()
    {
        IProducer<string> sp = new StringProducer();
        IProducer<object> op = sp;          // covariant assignment: string -> object
        object produced = op.Produce();
        Console.WriteLine($"covariant produce -> {produced}");

        IConsumer<object> oc = new ObjectConsumer();
        IConsumer<string> sc = oc;          // contravariant assignment
        sc.Consume("hello");
    }
}
