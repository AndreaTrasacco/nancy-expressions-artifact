using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsSubAdditiveVisitor : ICurveExpressionVisitor
{
    public bool IsSubAdditive;

    public void Visit(ConcreteCurveExpression expression) => IsSubAdditive = expression.Value.IsSubAdditive;

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsSubAdditive = expression.Compute().IsSubAdditive;

    public void Visit(NegateExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToNonNegativeExpression expression) => _throughCurveComputation(expression);

    public void Visit(SubAdditiveClosureExpression expression) => IsSubAdditive = true;

    public void Visit(SuperAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToUpperNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLowerNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLeftContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToRightContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(WithZeroOriginExpression expression) => _throughCurveComputation(expression);

    public void Visit(LowerPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(UpperPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(AdditionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsSubAdditive)
                break;
        }

        if (!IsSubAdditive) _throughCurveComputation(expression);
    }

    public void Visit(SubtractionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MinimumExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaximumExpression expression) => _throughCurveComputation(expression);

    public void Visit(ConvolutionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsSubAdditive)
                break;
        }

        if (!IsSubAdditive) _throughCurveComputation(expression);
    }

    public void Visit(DeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusConvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusDeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(CompositionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DelayByExpression expression) => expression.LeftExpression.Accept(this);

    public void Visit(AnticipateByExpression expression) => expression.LeftExpression.Accept(this);

    public void Visit(CurvePlaceholderExpression expression)
        => throw new InvalidOperationException(GetType() + ": Cannot perform the check on a placeholder expression!");

    public void Visit(ScaleExpression expression)
    {
        if (expression.RightExpression.Compute() > 0) expression.LeftExpression.Accept(this);
        else _throughCurveComputation(expression);
    }
}