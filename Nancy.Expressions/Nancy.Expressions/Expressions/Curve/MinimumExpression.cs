using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class MinimumExpression : CurveNAryExpression
{
    public MinimumExpression(IReadOnlyCollection<IGenericExpression<Curve>> expressions, string expressionName = "",
        ExpressionSettings? settings = null) :
        base(expressions, expressionName, settings)
    {
    }

    public MinimumExpression(IReadOnlyCollection<Curve> curves,
        IReadOnlyCollection<string> names, string expressionName = "", ExpressionSettings? settings = null) : base(
        curves, names, expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}