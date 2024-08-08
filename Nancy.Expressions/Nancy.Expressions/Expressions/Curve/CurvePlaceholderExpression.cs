using Unipi.Nancy.Expressions.Visitors;

namespace Unipi.Nancy.Expressions.Internals;

public class CurvePlaceholderExpression(
    string curveName,
    ExpressionSettings? settings = null) : CurveExpression(curveName, settings)
{
    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}