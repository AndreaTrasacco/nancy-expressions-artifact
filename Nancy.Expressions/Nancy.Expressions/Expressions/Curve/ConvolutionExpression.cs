using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class ConvolutionExpression : CurveNAryExpression
{
    static ConvolutionExpression()
    {
        AddEquivalence(typeof(ConvolutionExpression), new ConvolutionSubAdditiveWithDominance());
        AddEquivalence(typeof(ConvolutionExpression), new ConvSubAdditiveAsSelfConvMinimum());
        AddEquivalence(typeof(ConvolutionExpression), new ConvolutionDistributivityMin());
        AddEquivalence(typeof(ConvolutionExpression), new ConvAdditionByAConstant());
        AddEquivalence(typeof(ConvolutionExpression), new ConvAndSubadditiveClosure());
        AddEquivalence(typeof(ConvolutionExpression), new ConvolutionWithConcaveFunctions());
        AddEquivalence(typeof(ConvolutionExpression), new SelfConvolutionSubAdditive());
    }

    public ConvolutionExpression(IReadOnlyCollection<IGenericExpression<Curve>> expressions,
        string expressionName = "", ExpressionSettings? settings = null) : base(expressions, expressionName, settings)
    {
    }

    public ConvolutionExpression(IReadOnlyCollection<Curve> curves,
        IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null) : base(curves, names, expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}