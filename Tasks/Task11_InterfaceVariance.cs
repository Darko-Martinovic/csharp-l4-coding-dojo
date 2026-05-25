using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task11_InterfaceVariance : ICodingTask
{
    public int Id => 11;
    public string Title => "Make generic interfaces co-/contravariant";
    public string Category => "Delegates & Variance";

    public string Description => """
        Two assignments fail to compile:
          IProducer<object>  op = aStringProducer;   // want covariance
          IConsumer<string>  sc = anObjectConsumer;  // want contravariance

        Annotate the interfaces so both conversions are legal, and explain the
        rule that decides which annotation is allowed.
        """;

    public string StarterCode => """
        using System;

        interface IProducer<T> { T Produce(); }          // only returns T
        interface IConsumer<T> { void Consume(T item); } // only accepts T

        class StringProducer : IProducer<string> {
            public string Produce() => "hello";
        }
        class ObjectConsumer : IConsumer<object> {
            public void Consume(object item) => Console.WriteLine(item);
        }

        public class Program {
            public static void Main() {
                IProducer<string> sp = new StringProducer();
                IProducer<object> op = sp;   // CS0266 without variance

                IConsumer<object> oc = new ObjectConsumer();
                IConsumer<string> sc = oc;   // CS0266 without variance
            }
        }
        """;

    public string SolutionExplanation => """
        Variance is about substitutability of the constructed types:
        • A producer only *outputs* T. If it can hand you a string it can
          certainly satisfy a request for an object — so IProducer<string> may
          act as IProducer<object>. Mark T `out` (covariant); T may then appear
          only in output positions (return values).
        • A consumer only *accepts* T. Something that can swallow any object can
          certainly swallow a string — so IConsumer<object> may act as
          IConsumer<string>. Mark T `in` (contravariant); T may appear only in
          input positions (parameters).
        The compiler enforces that T is used only in the permitted positions.
        """;

    public string SolutionCode => """
        interface IProducer<out T> { T Produce(); }       // covariant
        interface IConsumer<in T>  { void Consume(T item); } // contravariant

        IProducer<string> sp = new StringProducer();
        IProducer<object> op = sp;   // OK: string -> object
        object value = op.Produce();

        IConsumer<object> oc = new ObjectConsumer();
        IConsumer<string> sc = oc;   // OK: object-consumer accepts strings
        sc.Consume("hello");
        """;
}
