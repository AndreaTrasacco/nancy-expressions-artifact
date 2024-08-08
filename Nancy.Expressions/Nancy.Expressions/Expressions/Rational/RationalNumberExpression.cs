using System.Runtime.CompilerServices;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

public class RationalNumberExpression : RationalExpression
{
    public RationalNumberExpression(Rational number,
        [CallerArgumentExpression("number")] string expressionName = "", 
        ExpressionSettings? settings = null) : base(expressionName, settings)
    {
        _value = number;
    }

    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}