using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions;

public abstract class CurveBinaryExpression<T1, T2>(
    IGenericExpression<T1> leftExpression,
    IGenericExpression<T2> rightExpression,
    string expressionName = "", 
    ExpressionSettings? settings = null)
    : CurveExpression(expressionName, settings), IGenericBinaryExpression<T1, T2, Curve>
{
    public IGenericExpression<T1> LeftExpression { get; } = leftExpression;
    public IGenericExpression<T2> RightExpression { get; } = rightExpression;
}