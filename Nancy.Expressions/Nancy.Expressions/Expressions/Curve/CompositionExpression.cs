using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class CompositionExpression(
    CurveExpression leftExpression,
    CurveExpression rightExpression,
    string expressionName = "",
    ExpressionSettings? settings = null)
    : CurveBinaryExpression<Curve, Curve>(leftExpression, rightExpression, expressionName, settings)
{
    public CompositionExpression(Curve curveL, string nameL, Curve curveR, string nameR,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), new ConcreteCurveExpression(curveR, nameR), expressionName,
            settings)
    {
    }

    public CompositionExpression(Curve curveL, string nameL, CurveExpression rightExpression,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), rightExpression, expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}