using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task01_GenericDelegateVariance : ICodingTask
{
    public int Id => 1;
    public string Title => "Generic delegate assignment fails to compile";
    public string Category => "Delegates & Variance";

    public string Description => """
        The code below fails to compile due to a delegate assignment error
        involving generic type parameters.

        1. Identify the cause of the compile-time error.
        2. Modify the code to make it compile successfully.
        3. Ensure the runtime behaviour remains correct.
        """;

    public string StarterCode => """
        using System;

        class MyClass {
          public override string ToString() => "I am MyClass";
        }
        delegate string ToString<T>(T param);

        public class Program {
          public static void Main() {
            ToString<object>  toObj = delegate (object param)  { return param.ToString(); };
            ToString<MyClass> toStr = delegate (MyClass param) { return "MyClass"; };

            Console.WriteLine(toStr(new MyClass()));   // before reassignment
            toStr = toObj;                             // <-- CS0029 here
            Console.WriteLine(toStr(new MyClass()));   // after reassignment
          }
        }
        """;

    public string SolutionExplanation => """
        Constructed generic delegate types are invariant by default, so
        ToString<object> and ToString<MyClass> are unrelated types and the
        assignment `toStr = toObj` is rejected (CS0029).

        The conversion you want is contravariance: a delegate that accepts a
        less-derived parameter (object) can stand in for one that accepts a
        more-derived parameter (MyClass), because it can handle anything the
        caller passes. Mark the type parameter `in` to opt into that. No
        runtime logic changes — toObj simply calls the real ToString().
        """;

    public string SolutionCode => """
        class MyClass {
          public override string ToString() => "I am MyClass";
        }

        // 'in' makes T contravariant: ToString<object> is assignable
        // to a ToString<MyClass> variable.
        delegate string ToString<in T>(T param);

        public class Program {
          public static void Main() {
            ToString<object>  toObj = param => param.ToString();
            ToString<MyClass> toStr = param => "MyClass";

            Console.WriteLine(toStr(new MyClass()));   // MyClass
            toStr = toObj;                             // now legal
            Console.WriteLine(toStr(new MyClass()));   // I am MyClass
          }
        }
        """;
}
