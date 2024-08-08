using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class DeconvolutionExpression(
    CurveExpression leftExpression,
    CurveExpression rightExpression,
    string expressionName = "", ExpressionSettings? settings = null)
    : CurveBinaryExpression<Curve, Curve>(leftExpression, rightExpression, expressionName, settings)
{
    static DeconvolutionExpression()
    {
        AddEquivalence(typeof(DeconvolutionExpression), new DeconvolutionWithConvolution());
        AddEquivalence(typeof(DeconvolutionExpression), new DeconvolutionWeakCommutativity());
        AddEquivalence(typeof(DeconvolutionExpression), new DeconvDistributivityWithMax());
        AddEquivalence(typeof(DeconvolutionExpression), new DeconvDistributivityWithMin());
        AddEquivalence(typeof(DeconvolutionExpression), new DeconvAndSubAdditiveClosure());
        AddEquivalence(typeof(DeconvolutionExpression), new SelfDeconvolutionSubAdditive());
    }
    
    public DeconvolutionExpression(Curve curveL, string nameL, Curve curveR, string nameR,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), new ConcreteCurveExpression(curveR, nameR), expressionName, settings)
    {
    }

    public DeconvolutionExpression(Curve curveL, string nameL, CurveExpression rightExpression,
        string expressionName = "", ExpressionSettings? settings = null) :
        this(new ConcreteCurveExpression(curveL, nameL), rightExpression, expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}