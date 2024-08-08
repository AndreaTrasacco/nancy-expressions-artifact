using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions;

public abstract class RationalUnaryExpression<T>(
    IGenericExpression<T> expression,
    string expressionName = "", 
    ExpressionSettings? settings = null)
    : RationalExpression(expressionName, settings), IGenericUnaryExpression<T, Rational>
{
    public IGenericExpression<T> Expression { get; } = expression;
}