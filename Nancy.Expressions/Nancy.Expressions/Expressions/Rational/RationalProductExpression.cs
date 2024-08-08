using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

public class RationalProductExpression : RationalNAryExpression
{
    public RationalProductExpression(IReadOnlyCollection<IGenericExpression<Rational>> expressions,
        string expressionName = "", ExpressionSettings? settings = null) : base(expressions, expressionName, settings)
    {
    }

    public RationalProductExpression(IReadOnlyCollection<Rational> rationals,
        IReadOnlyCollection<string> names, string expressionName = "", ExpressionSettings? settings = null) : base(rationals, names, expressionName, settings)
    {
    }

    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}