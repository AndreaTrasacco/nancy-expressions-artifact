using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class VerticalDeviationExpression(
    CurveExpression leftExpression,
    CurveExpression rightExpression,
    string expressionName = "",
    ExpressionSettings? settings = null)
    : RationalBinaryExpression<Curve, Curve>(leftExpression, rightExpression, expressionName, settings)
{
    public VerticalDeviationExpression(Curve curveL, string nameL, Curve curveR, string nameR,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), new ConcreteCurveExpression(curveR, nameR), expressionName,
            settings)
    {
    }

    public VerticalDeviationExpression(Curve curveL, string nameL, CurveExpression rightExpression,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), rightExpression, expressionName, settings)
    {
    }

    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}