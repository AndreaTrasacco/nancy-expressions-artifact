using System.Runtime.CompilerServices;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

/// <summary>
/// Class describing an expression composed of a rational (<see cref="Rational"/>) number
/// </summary>
public class RationalNumberExpression : RationalExpression
{
    /// <summary>
    /// Creates a rational number expression starting from a <see cref="Rational"/> object
    /// </summary>
    public RationalNumberExpression(Rational number,
        [CallerArgumentExpression("number")] string expressionName = "", 
        ExpressionSettings? settings = null) : base(expressionName, settings)
    {
        _value = number;
    }

    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}