using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class LowerPseudoInverseExpression(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : CurveUnaryExpression<Curve>(expression, expressionName, settings)
{
    static LowerPseudoInverseExpression()
    {
        AddEquivalence(typeof(LowerPseudoInverseExpression), new IsomorphismConvRight());
        AddEquivalence(typeof(LowerPseudoInverseExpression), new PseudoInverseOfLeftContinuous());
    }
    
    public LowerPseudoInverseExpression(Curve curve, string name, string expressionName = "", ExpressionSettings? settings = null) : this(
        new ConcreteCurveExpression(curve, name), expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}