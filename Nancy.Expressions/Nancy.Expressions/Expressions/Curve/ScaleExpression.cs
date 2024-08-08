using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

public class ScaleExpression(
    CurveExpression leftExpression,
    RationalExpression rightExpression,
    string expressionName = "", ExpressionSettings? settings = null)
    : CurveBinaryExpression<Curve, Rational>(leftExpression, rightExpression, expressionName, settings)
{
    public ScaleExpression(Curve curveL, string nameL, Rational delay, string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), new RationalNumberExpression(delay), expressionName, settings)
    {
    }

    public ScaleExpression(Curve curveL, string nameL, RationalExpression rightExpression,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), rightExpression, expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}