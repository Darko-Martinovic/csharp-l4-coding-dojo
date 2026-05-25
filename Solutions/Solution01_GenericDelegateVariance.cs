using CodingTasks.Core;

namespace CodingTasks.Solutions.S01;

// 'in' makes T contravariant: a Printer<object> is assignable to a
// Printer<MyClass> variable, because it can handle anything the caller passes.
public delegate string Printer<in T>(T param);

public sealed class MyClass
{
    public override string ToString() => "I am MyClass";
}

public sealed class Solution01_GenericDelegateVariance : ISolution
{
    public int Id => 1;

    public void Run()
    {
        Printer<object> toObj = param => param!.ToString()!;
        Printer<MyClass> toStr = _ => "MyClass";

        Console.WriteLine(toStr(new MyClass()));   // before reassignment
        toStr = toObj;                             // legal thanks to 'in T'
        Console.WriteLine(toStr(new MyClass()));   // after reassignment
    }
}
