using Nancy.Expressions.Expressions;
using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions;

public abstract class
    RationalNAryExpression : RationalExpression, IGenericNAryExpression<Rational, Rational> // For operators on rationals that are commutative and associative
{
    public IReadOnlyCollection<IGenericExpression<Rational>> Expressions { get; }

    public RationalNAryExpression(
        IReadOnlyCollection<IGenericExpression<Rational>> expressions,
        string expressionName = "", ExpressionSettings? settings = null) : base(expressionName, settings)
    {
        Expressions = expressions;
    }

    public RationalNAryExpression(
        IReadOnlyCollection<Rational> rationals,
        IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null) : base(expressionName, settings)
    {
        List<IGenericExpression<Rational>> expressions = [];
        foreach (var (rational, name) in rationals.Zip(names, (c, n) => (curve: c, name: n)))
            expressions.Add(new RationalNumberExpression(rational, name));
        Expressions = expressions;
    }

    public RationalExpression Append(IGenericExpression<Rational> expression, string expressionName = "", ExpressionSettings? settings = null)
    {
        if (GetType() == expression.GetType())
            return (RationalExpression)Activator.CreateInstance(GetType(),
                (IReadOnlyCollection<IGenericExpression<Rational>>)
                [.. Expressions, .. ((RationalNAryExpression)expression).Expressions], expressionName, settings)!;
        return (RationalExpression)Activator.CreateInstance(GetType(),
            (IReadOnlyCollection<IGenericExpression<Rational>>) [.. Expressions, expression], expressionName, settings)!;
    }
}