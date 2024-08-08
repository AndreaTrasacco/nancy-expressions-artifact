using Unipi.Nancy.Expressions.ExpressionsUtility;
using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Visitors;

public class IsZeroAtZeroVisitor : ICurveExpressionVisitor
{
    public bool IsZeroAtZero;

    public void Visit(ConcreteCurveExpression expression) =>
        IsZeroAtZero = expression.Value.IsZeroAtZero();

    private void _throughCurveComputation(IGenericExpression<Curve> expression) =>
        IsZeroAtZero = expression.Compute().IsZeroAtZero();
    
    public void Visit(NegateExpression expression)
    {
        expression.Expression.Accept(this);
    }

    public void Visit(ToNonNegativeExpression expression)
    {
        IsZeroAtZero = expression.Expression.Compute().ValueAt(Rational.Zero) <= Rational.Zero;
    }

    public void Visit(SubAdditiveClosureExpression expression)
    {
        // The SAC is 0 in 0 only if the argument is >= 0 in 0
        expression.Expression.Accept(this);
        if (!IsZeroAtZero)
        {
            IsZeroAtZero = expression.Expression.Value.ValueAt(Rational.Zero) > Rational.Zero;
        }
    }

    public void Visit(SuperAdditiveClosureExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToUpperNonDecreasingExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToLowerNonDecreasingExpression expression) => _throughCurveComputation(expression);
    
    public void Visit(ToLeftContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(ToRightContinuousExpression expression) => _throughCurveComputation(expression);

    public void Visit(WithZeroOriginExpression expression) => IsZeroAtZero = true;

    public void Visit(LowerPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(UpperPseudoInverseExpression expression) => _throughCurveComputation(expression);

    public void Visit(AdditionExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsZeroAtZero)
                break;
        }
        if(!IsZeroAtZero) _throughCurveComputation(expression);
    }

    public void Visit(SubtractionExpression expression)
    {
        expression.LeftExpression.Accept(this);
        if (IsZeroAtZero)
        {
            expression.RightExpression.Accept(this);
        }
        
        if (!IsZeroAtZero) _throughCurveComputation(expression);
    }

    public void Visit(MinimumExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsZeroAtZero)
                break;
        }
        if(!IsZeroAtZero) _throughCurveComputation(expression);
    }

    public void Visit(MaximumExpression expression)
    {
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            if (!IsZeroAtZero)
                break;
        }
        if(!IsZeroAtZero) _throughCurveComputation(expression);
    }

    public void Visit(ConvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusConvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(MaxPlusDeconvolutionExpression expression) => _throughCurveComputation(expression);

    public void Visit(CompositionExpression expression) => _throughCurveComputation(expression);

    public void Visit(DelayByExpression expression) => expression.LeftExpression.Accept(this);

    public void Visit(AnticipateByExpression expression) => _throughCurveComputation(expression);

    public void Visit(CurvePlaceholderExpression expression)
        => throw new InvalidOperationException(GetType() + ": Cannot perform the check on a placeholder expression!");

    public void Visit(ScaleExpression expression)
    {
        if (expression.RightExpression.Compute() == 0) IsZeroAtZero = true;
        else expression.LeftExpression.Accept(this);
    }
}