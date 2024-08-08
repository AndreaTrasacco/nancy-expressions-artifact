using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsNonNegativeVisitor : ICurveExpressionVisitor
{
    public bool IsNonNegative;
    
    public void Visit(ConcreteCurveExpression expression) => IsNonNegative = expression.Value.IsNonNegative;

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsNonNegative = expression.Compute().IsNonNegative;

    public void Visit(NegateExpression expression)
    {
        expression.Expression.Accept(this);
        // If the argument is "NonNegative" then the negation of it won't be "NonNegative"
        if (IsNonNegative) IsNonNegative = false;
        else _throughCurveComputation(expression);
    }

    public void Visit(ToNonNegativeExpression expression) => IsNonNegative = true;

    public void Visit(SubAdditiveClosureExpression expression)
    {
        if (((CurveExpression)expression.Expression).IsNonDecreasing)
        {
            expression.Expression.Accept(this);
            if (IsNonNegative) return;
        }
        _throughCurveComputation(expression);
    }

    public void Visit(SuperAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToUpperNonDecreasingExpression expression)
    {
        expression.Expression.Accept(this);
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(ToLowerNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLeftContinuousExpression expression)
    {
        expression.Expression.Accept(this);
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(ToRightContinuousExpression expression)
    {
        expression.Expression.Accept(this);
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(WithZeroOriginExpression expression)
    {
        expression.Expression.Accept(this);
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(LowerPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(UpperPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(AdditionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            IsNonNegative = false;
            if (((CurveExpression)e).IsNonDecreasing)
            {
                e.Accept(this);
                if (!IsNonNegative)
                    break;
            }
            else break;
        }
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(SubtractionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MinimumExpression expression)
    {
        // If all operands are non negative --> then the minimum is non negative
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsNonNegative)
                break;
        }
    }

    public void Visit(MaximumExpression expression)
    {
        // If at least one operand is non negative --> then the maximum is non negative
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (IsNonNegative)
                break;
        }
    }

    public void Visit(ConvolutionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            IsNonNegative = false;
            if (((CurveExpression)e).IsNonDecreasing)
            {
                e.Accept(this);
                if (!IsNonNegative)
                    break;
            }
            else break;
        }
        if(!IsNonNegative) _throughCurveComputation(expression);
    }

    public void Visit(DeconvolutionExpression expression)
    {
        if (((CurveExpression)expression.LeftExpression).IsNonDecreasing)
        {
            expression.LeftExpression.Accept(this);
            if (IsNonNegative)
            {
                // expression._isNonNegative = true;
                // expression._isNonDecreasing = true;
                return;
            }
        }

        _throughCurveComputation(expression);
    }

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