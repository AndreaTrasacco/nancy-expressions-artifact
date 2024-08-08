using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsRightContinuousVisitor : ICurveExpressionVisitor
{
    public bool IsRightContinuous;
    
    public void Visit(ConcreteCurveExpression expression)
    {
        IsRightContinuous = expression.Value.IsRightContinuous;
    }

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsRightContinuous = expression.Compute().IsRightContinuous;
    
    public void Visit(NegateExpression expression) => expression.Expression.Accept(this);

    public void Visit(ToNonNegativeExpression expression) => _throughCurveComputation(expression);

    public void Visit(SubAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(SuperAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToUpperNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLowerNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLeftContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToRightContinuousExpression expression) => IsRightContinuous = true;

    public void Visit(WithZeroOriginExpression expression) => _throughCurveComputation(expression);

    public void Visit(LowerPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(UpperPseudoInverseExpression expression)
    {
        if (((CurveExpression)expression.Expression).IsNonDecreasing)
            IsRightContinuous = true;
        else
            _throughCurveComputation(expression);
    }

    public void Visit(AdditionExpression expression) => _throughCurveComputation(expression);

    public void Visit(SubtractionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MinimumExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaximumExpression expression) => _throughCurveComputation(expression);

    public void Visit(ConvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusConvolutionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            IsRightContinuous = false;
            if (((CurveExpression)e).IsNonDecreasing)
            {
                e.Accept(this);
                if (!IsRightContinuous)
                    break;
            }
            else break;
        }
        if(!IsRightContinuous) _throughCurveComputation(expression);
    }

    public void Visit(MaxPlusDeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(CompositionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DelayByExpression expression) => _throughCurveComputation(expression);
    
    public void Visit(AnticipateByExpression expression) => _throughCurveComputation(expression);
    
    public void Visit(CurvePlaceholderExpression expression)
        => throw new InvalidOperationException(GetType() + ": Cannot perform the check on a placeholder expression!");

    public void Visit(ScaleExpression expression)
    {
        if (expression.RightExpression.Compute() != 0) expression.LeftExpression.Accept(this);
        else IsRightContinuous = true;
    }
}