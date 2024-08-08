using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsConvexVisitor : ICurveExpressionVisitor
{
    public bool IsConvex;

    public void Visit(ConcreteCurveExpression expression) => IsConvex = expression.Value.IsConvex;

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsConvex = expression.Compute().IsConvex;
    
    public void Visit(NegateExpression expression)
    {
        expression.Expression.Accept(this);
        if(IsConvex)
            IsConvex = !IsConvex;
        else
            IsConvex = expression.Compute().IsConvex;
    }

    public void Visit(ToNonNegativeExpression expression) => _throughCurveComputation(expression);

    public void Visit(SubAdditiveClosureExpression expression) => _throughCurveComputation(expression);

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
            if (!IsConvex)
                break;
        }

        if (!IsConvex) _throughCurveComputation(expression);
    }

    public void Visit(SubtractionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MinimumExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaximumExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsConvex)
                break;
        }

        if (!IsConvex) _throughCurveComputation(expression);
    }

    public void Visit(ConvolutionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsConvex)
                break;
        }

        if (!IsConvex) _throughCurveComputation(expression);
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
        if (expression.RightExpression.Compute() > 0) expression.LeftExpression.Accept(this);
        else _throughCurveComputation(expression);
    }
}