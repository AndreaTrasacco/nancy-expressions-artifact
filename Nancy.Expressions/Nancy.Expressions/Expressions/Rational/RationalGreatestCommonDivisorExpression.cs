using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

public class RationalGreatestCommonDivisorExpression : RationalNAryExpression
{
    public RationalGreatestCommonDivisorExpression(IReadOnlyCollection<IGenericExpression<Rational>> expressions,
        string expressionName = "", ExpressionSettings? settings = null) : base(expressions, expressionName, settings)
    {
    }

    public RationalGreatestCommonDivisorExpression(IReadOnlyCollection<Rational> rationals,
        IReadOnlyCollection<string> names, string expressionName = "", ExpressionSettings? settings = null) : base(rationals, names, expressionName, settings)
    {
    }

    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}