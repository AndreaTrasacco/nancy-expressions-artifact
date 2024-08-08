using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class SubAdditiveClosureExpression(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : CurveUnaryExpression<Curve>(expression, expressionName, settings)
{
    static SubAdditiveClosureExpression()
    {
        AddEquivalence(typeof(SubAdditiveClosureExpression), new SubAdditiveClosureOfMin());
        AddEquivalence(typeof(SubAdditiveClosureExpression), new SubAdditiveClosureOfSubAdd());
    }
    
    public SubAdditiveClosureExpression(Curve curve, string name, string expressionName = "", ExpressionSettings? settings = null) : this(
        new ConcreteCurveExpression(curve, name), expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}