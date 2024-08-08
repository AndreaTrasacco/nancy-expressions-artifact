using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Visitors;

/// <summary>
/// Visitor class used to compute the value of a curve expression.
/// </summary>
public class CurveExpressionEvaluator : ICurveExpressionVisitor
{
    private Curve _result = Curve.Zero();

    public Curve GetResult(CurveExpression expression)
    {
        expression.Accept(this);
        return _result;
    }

    public void Visit(ConcreteCurveExpression expression) => _result = expression.Value;

    private void VisitUnary(CurveUnaryExpression<Curve> expression, Func<Curve, Curve> operation)
        => _result = operation(expression.Expression.Value);

    private void VisitBinary(CurveBinaryExpression<Curve, Curve> expression,
        Func<Curve, Curve, Curve> operation)
        => _result = operation(expression.LeftExpression.Value, expression.RightExpression.Value);

    private void VisitNAry(CurveNAryExpression expression, Func<IReadOnlyCollection<Curve>, Curve> operation)
    {
        List<Curve> curves = [];
        curves.AddRange(expression.Expressions.Select(e => e.Value));

        _result = operation(curves);
    }

    public void Visit(NegateExpression expression)
        => VisitUnary(expression, curve => curve.Negate());

    public void Visit(ToNonNegativeExpression expression)
        => VisitUnary(expression, curve => curve.ToNonNegative());

    public void Visit(SubAdditiveClosureExpression expression)
        => VisitUnary(expression, curve => curve.SubAdditiveClosure(expression.Settings?.ComputationSettings));

    public void Visit(SuperAdditiveClosureExpression expression)
        => VisitUnary(expression, curve => curve.SuperAdditiveClosure(expression.Settings?.ComputationSettings));

    public void Visit(ToUpperNonDecreasingExpression expression)
        => VisitUnary(expression, curve => curve.ToUpperNonDecreasing());

    public void Visit(ToLowerNonDecreasingExpression expression)
        => VisitUnary(expression, curve => curve.ToLowerNonDecreasing());

    public void Visit(ToLeftContinuousExpression expression)
        => VisitUnary(expression, curve => curve.ToLeftContinuous());

    public void Visit(ToRightContinuousExpression expression)
        => VisitUnary(expression, curve => curve.ToRightContinuous());

    public void Visit(WithZeroOriginExpression expression)
        => VisitUnary(expression, curve => curve.WithZeroOrigin());

    public void Visit(LowerPseudoInverseExpression expression)
        => VisitUnary(expression, curve => curve.LowerPseudoInverse());

    public void Visit(UpperPseudoInverseExpression expression)
        => VisitUnary(expression, curve => curve.UpperPseudoInverse());

    public void Visit(AdditionExpression expression)
        => VisitNAry(expression, curves => Curve.Addition(curves, expression.Settings?.ComputationSettings));

    public void Visit(SubtractionExpression expression)
        => VisitBinary(expression, (leftCurve, rightCurve) => Curve.Subtraction(leftCurve, rightCurve));

    public void Visit(MinimumExpression expression)
        => VisitNAry(expression, curves => Curve.Minimum(curves, expression.Settings?.ComputationSettings));

    public void Visit(MaximumExpression expression)
        => VisitNAry(expression, curves => Curve.Maximum(curves, expression.Settings?.ComputationSettings));

    public void Visit(ConvolutionExpression expression)
        => VisitNAry(expression, curves => Curve.Convolution(curves, expression.Settings?.ComputationSettings));

    public void Visit(DeconvolutionExpression expression)
        => VisitBinary(expression, (leftCurve, rightCurve) => Curve.Deconvolution(leftCurve, rightCurve, expression.Settings?.ComputationSettings));

    public void Visit(MaxPlusConvolutionExpression expression)
        => VisitNAry(expression, curves => Curve.MaxPlusConvolution(curves, expression.Settings?.ComputationSettings));

    public void Visit(MaxPlusDeconvolutionExpression expression)
        => VisitBinary(expression, (leftCurve, rightCurve) => Curve.MaxPlusDeconvolution(leftCurve, rightCurve, expression.Settings?.ComputationSettings));

    public void Visit(CompositionExpression expression)
        => VisitBinary(expression, (leftCurve, rightCurve) => Curve.Composition(leftCurve, rightCurve, expression.Settings?.ComputationSettings));

    public void Visit(DelayByExpression expression)
        => _result = expression.LeftExpression.Value.DelayBy(expression.RightExpression.Value);

    public void Visit(AnticipateByExpression expression)
        => _result = expression.LeftExpression.Value.AnticipateBy(expression.RightExpression.Value);

    public void Visit(CurvePlaceholderExpression expression)
        => throw new InvalidOperationException("Can't evaluate an expression with placeholders!");

    public void Visit(ScaleExpression expression)
        => _result = expression.LeftExpression.Value.Scale(expression.RightExpression.Value);
}