using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions.Visitors;

public interface ICurveExpressionVisitor : IExpressionVisitor
{
    public void Visit(ConcreteCurveExpression expression);
    public void Visit(NegateExpression expression);
    public void Visit(ToNonNegativeExpression expression);
    public void Visit(SubAdditiveClosureExpression expression);
    public void Visit(SuperAdditiveClosureExpression expression);
    public void Visit(ToUpperNonDecreasingExpression expression);
    public void Visit(ToLowerNonDecreasingExpression expression);
    public void Visit(ToLeftContinuousExpression expression);
    public void Visit(ToRightContinuousExpression expression);
    public void Visit(WithZeroOriginExpression expression);
    public void Visit(LowerPseudoInverseExpression expression);
    public void Visit(UpperPseudoInverseExpression expression);
    public void Visit(AdditionExpression expression);
    public void Visit(SubtractionExpression expression);
    public void Visit(MinimumExpression expression);
    public void Visit(MaximumExpression expression);
    public void Visit(ConvolutionExpression expression);
    public void Visit(DeconvolutionExpression expression);
    public void Visit(MaxPlusConvolutionExpression expression);
    public void Visit(MaxPlusDeconvolutionExpression expression);
    public void Visit(CompositionExpression expression);
    public void Visit(DelayByExpression expression);
    public void Visit(AnticipateByExpression expression);
    public void Visit(CurvePlaceholderExpression expression);
    public void Visit(ScaleExpression expression);
}