using System.Linq.Expressions;
using CodingTasks.Core;

namespace CodingTasks.Solutions.S29;

public sealed class Solution29_ExpressionTrees : ISolution
{
    public int Id => 29;

    public void Run()
    {
        // Expression<Func<...>> is a data tree you can inspect (as ORMs do to
        // build SQL); call .Compile() to get the runnable delegate.
        Expression<Func<int, bool>> rule = x => x > 5;

        var body = (BinaryExpression)rule.Body;
        Console.WriteLine($"inspected rule: {body.Left} {body.NodeType} {body.Right}");
        Console.WriteLine($"full tree     : {rule}");

        var compiled = rule.Compile();
        Console.WriteLine($"compiled(10)  : {compiled(10)}");
        Console.WriteLine($"compiled(3)   : {compiled(3)}");
    }
}
