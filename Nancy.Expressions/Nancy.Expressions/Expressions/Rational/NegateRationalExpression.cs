using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

public class NegateRationalExpression(RationalExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : RationalUnaryExpression<Rational>(expression, expressionName, settings)
{
    public NegateRationalExpression(Rational number, string expressionName = "", ExpressionSettings? settings = null) : this(
        new RationalNumberExpression(number), expressionName, settings)
    {
    }
    
    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}