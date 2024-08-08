using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsLeftContinuousVisitor : ICurveExpressionVisitor
{
    public bool IsLeftContinuous;
    
    public void Visit(ConcreteCurveExpression expression) => IsLeftContinuous = expression.Value.IsLeftContinuous;

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsLeftContinuous = expression.Compute().IsLeftContinuous;

    public void Visit(NegateExpression expression) => expression.Expression.Accept(this);

    public void Visit(ToNonNegativeExpression expression) => _throughCurveComputation(expression);
    
    public void Visit(SubAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(SuperAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToUpperNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLowerNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLeftContinuousExpression expression) => IsLeftContinuous = true;

    public void Visit(ToRightContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(WithZeroOriginExpression expression) => _throughCurveComputation(expression);

    public void Visit(LowerPseudoInverseExpression expression)
    {
        if (((CurveExpression)expression.Expression).IsNonDecreasing)
            IsLeftContinuous = true;
        else
            _throughCurveComputation(expression);
    }

    public void Visit(UpperPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(AdditionExpression expression) => _throughCurveComputation(expression);

    public void Visit(SubtractionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MinimumExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaximumExpression expression) => _throughCurveComputation(expression);

    public void Visit(ConvolutionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            IsLeftContinuous = false;
            if (((CurveExpression)e).IsNonDecreasing)
            {
                e.Accept(this);
                if (!IsLeftContinuous)
                    break;
            }
            else break;
        }
        if(!IsLeftContinuous) _throughCurveComputation(expression);
    }

    public void Visit(DeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusConvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusDeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(CompositionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DelayByExpression expression) => _throughCurveComputation(expression);

    public void Visit(AnticipateByExpression expression) => _throughCurveComputation(expression);

    public void Visit(CurvePlaceholderExpression expression)
        => throw new InvalidOperationException(GetType() + ": Cannot perform the check on a placeholder expression!");
    
    public void Visit(ScaleExpression expression)
    {
        if (expression.RightExpression.Compute() != 0) expression.LeftExpression.Accept(this);
        else IsLeftContinuous = true;
    }
}