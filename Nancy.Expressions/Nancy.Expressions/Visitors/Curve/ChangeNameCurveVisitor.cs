using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

/// <summary>
/// Visitor class used to update the name of a curve expression.
/// </summary>
/// <param name="newName">The new name of the expression</param>
public class ChangeNameCurveVisitor(string newName) : ICurveExpressionVisitor
{
    public CurveExpression Result = Expressions.FromCurve(Curve.Zero());

    public void Visit(ConcreteCurveExpression expression)
        => Result = Expressions.FromCurve(expression.Value, newName);

    public void Visit(NegateExpression expression)
        => Result = Expressions.Negate((CurveExpression)expression.Expression, newName);

    public void Visit(ToNonNegativeExpression expression)
        => Result = Expressions.ToNonNegative((CurveExpression)expression.Expression, newName);

    public void Visit(SubAdditiveClosureExpression expression)
        => Result = Expressions.SubAdditiveClosure((CurveExpression)expression.Expression, newName);

    public void Visit(SuperAdditiveClosureExpression expression)
        => Result = Expressions.SuperAdditiveClosure((CurveExpression)expression.Expression, newName);

    public void Visit(ToUpperNonDecreasingExpression expression)
        => Result = Expressions.ToUpperNonDecreasing((CurveExpression)expression.Expression, newName);

    public void Visit(ToLowerNonDecreasingExpression expression)
        => Result = Expressions.ToLowerNonDecreasing((CurveExpression)expression.Expression, newName);

    public void Visit(ToLeftContinuousExpression expression)
        => Result = Expressions.ToLeftContinuous((CurveExpression)expression.Expression, newName);

    public void Visit(ToRightContinuousExpression expression)
        => Result = Expressions.ToRightContinuous((CurveExpression)expression.Expression, newName);

    public void Visit(WithZeroOriginExpression expression)
        => Result = Expressions.WithZeroOrigin((CurveExpression)expression.Expression, newName);

    public void Visit(LowerPseudoInverseExpression expression)
        => Result = Expressions.LowerPseudoInverse((CurveExpression)expression.Expression, newName);

    public void Visit(UpperPseudoInverseExpression expression)
        => Result = Expressions.UpperPseudoInverse((CurveExpression)expression.Expression, newName);

    public void Visit(AdditionExpression expression)
        => Result = new AdditionExpression(expression.Expressions, newName);


    public void Visit(SubtractionExpression expression)
        => Result = Expressions.Subtraction((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(MinimumExpression expression)
        => Result = new MinimumExpression(expression.Expressions, newName);

    public void Visit(MaximumExpression expression)
        => Result = new MaximumExpression(expression.Expressions, newName);


    public void Visit(ConvolutionExpression expression)
        => Result = new ConvolutionExpression(expression.Expressions, newName);


    public void Visit(DeconvolutionExpression expression)
        => Result = Expressions.Deconvolution((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(MaxPlusConvolutionExpression expression)
        => Result = new MaxPlusConvolutionExpression(expression.Expressions, newName);


    public void Visit(MaxPlusDeconvolutionExpression expression)
        => Result = Expressions.MaxPlusDeconvolution((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(CompositionExpression expression)
        => Result = Expressions.Composition((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(DelayByExpression expression)
        => Result = Expressions.DelayBy((CurveExpression)expression.LeftExpression,
            (RationalExpression)expression.RightExpression, newName);

    public void Visit(AnticipateByExpression expression)
        => Result = Expressions.AnticipateBy((CurveExpression)expression.LeftExpression,
            (RationalExpression)expression.RightExpression, newName);

    public void Visit(CurvePlaceholderExpression expression)
        => Result = new CurvePlaceholderExpression(newName);

    public void Visit(ScaleExpression expression)
        => Result = Expressions.Scale((CurveExpression)expression.LeftExpression,
            (RationalExpression)expression.RightExpression, newName);
}