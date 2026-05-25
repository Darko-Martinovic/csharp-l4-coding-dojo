using CodingTasks.Core;

namespace CodingTasks.Tasks;

public sealed class Task29_ExpressionTrees : ICodingTask
{
    public int Id => 29;
    public string Title => "Func vs Expression<Func>: inspectable predicates";
    public string Category => "Expressions";

    public string Description => """
        A repository takes a Func<T,bool> predicate and wants to log or
        translate it (e.g. to SQL), but a compiled delegate is an opaque blob —
        you can run it, not read it. Change the parameter so the predicate's
        structure can be inspected, then both examine it and execute it.
        """;

    public string StarterCode => """
        bool Matches(Func<int, bool> predicate, int value)
            => predicate(value);          // can only call it; can't see x > 5

        // We'd like to log "the rule was: x > 5" — impossible with Func.
        """;

    public string SolutionExplanation => """
        `Func<int,bool>` is already-compiled IL. `Expression<Func<int,bool>>` is
        a *data* representation of the lambda — a tree of nodes (Parameter
        expression, BinaryExpression GreaterThan, ConstantExpression 5) you can
        walk, rewrite, or translate. That's exactly how ORMs like EF Core turn
        `x => x.Age > 18` into SQL. When you actually need to run it, call
        .Compile() to get the delegate. The trade-off: building/compiling a tree
        costs more than a plain lambda, so use it only when you need inspection.
        """;

    public string SolutionCode => """
        Expression<Func<int, bool>> rule = x => x > 5;

        // inspect the structure
        var body = (BinaryExpression)rule.Body;
        Console.WriteLine($"left={body.Left}, op={body.NodeType}, right={body.Right}");
        // left=x, op=GreaterThan, right=5

        // execute it
        Func<int, bool> compiled = rule.Compile();
        Console.WriteLine(compiled(10));   // True
        Console.WriteLine(compiled(3));    // False
        """;
}
