using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions;

public abstract class CurveUnaryExpression<T>(
    IGenericExpression<T> expression,
    string expressionName = "", 
    ExpressionSettings? settings = null)
    : CurveExpression(expressionName, settings), IGenericUnaryExpression<T, Curve>
{
    public IGenericExpression<T> Expression { get; set; } = expression;
}