using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions;

public abstract class RationalBinaryExpression<T1, T2>(
    IGenericExpression<T1> leftExpression,
    IGenericExpression<T2> rightExpression,
    string expressionName = "", 
    ExpressionSettings? settings = null)
    : RationalExpression(expressionName, settings), IGenericBinaryExpression<T1, T2, Rational>
{
    public IGenericExpression<T1> LeftExpression { get; } = leftExpression;
    public IGenericExpression<T2> RightExpression { get; } = rightExpression;
}