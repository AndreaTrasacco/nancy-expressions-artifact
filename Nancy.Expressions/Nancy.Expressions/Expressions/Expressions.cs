using System.Runtime.CompilerServices;
using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions;

/// <summary>
/// Static class with functions to build NetCal expressions
/// </summary>
public static class Expressions
{
    public static ConcreteCurveExpression FromCurve(Curve curve, [CallerArgumentExpression("curve")] string name = "")
        => new ConcreteCurveExpression(curve, name);

    public static RationalNumberExpression FromRational(Rational number, [CallerArgumentExpression("number")] string name = "")
        => new RationalNumberExpression(number, name);

    public static CurveExpression Negate(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.Negate(expressionName, settings);

    public static CurveExpression Negate(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new NegateExpression(curve, name, expressionName, settings);

    public static CurveExpression ToNonNegative(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.ToNonNegative(expressionName, settings);

    public static CurveExpression ToNonNegative(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ToNonNegativeExpression(curve, name, expressionName, settings);

    public static CurveExpression SubAdditiveClosure(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.SubAdditiveClosure(expressionName, settings);

    public static CurveExpression SubAdditiveClosure(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new SubAdditiveClosureExpression(curve, name, expressionName, settings);

    public static CurveExpression SuperAdditiveClosure(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.SuperAdditiveClosure(expressionName, settings);

    public static CurveExpression SuperAdditiveClosure(Curve curve,
        [CallerArgumentExpression("curve")] string name = "", string expressionName = "", ExpressionSettings? settings = null)
        => new SuperAdditiveClosureExpression(curve, name, expressionName, settings);

    public static CurveExpression ToUpperNonDecreasing(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.ToUpperNonDecreasing(expressionName, settings);

    public static CurveExpression ToUpperNonDecreasing(Curve curve,
        [CallerArgumentExpression("curve")] string name = "", string expressionName = "", ExpressionSettings? settings = null)
        => new ToUpperNonDecreasingExpression(curve, name, expressionName, settings);

    public static CurveExpression ToLowerNonDecreasing(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.ToLowerNonDecreasing(expressionName, settings);

    public static CurveExpression ToLowerNonDecreasing(Curve curve,
        [CallerArgumentExpression("curve")] string name = "", string expressionName = "", ExpressionSettings? settings = null)
        => new ToLowerNonDecreasingExpression(curve, name, expressionName, settings);

    public static CurveExpression ToLeftContinuous(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.ToLeftContinuous(expressionName, settings);

    public static CurveExpression ToLeftContinuous(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ToLeftContinuousExpression(curve, name, expressionName, settings);

    public static CurveExpression ToRightContinuous(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.ToRightContinuous(expressionName, settings);

    public static CurveExpression ToRightContinuous(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ToRightContinuousExpression(curve, name, expressionName, settings);

    public static CurveExpression WithZeroOrigin(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.WithZeroOrigin(expressionName, settings);

    public static CurveExpression WithZeroOrigin(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new WithZeroOriginExpression(curve, name, expressionName, settings);

    public static CurveExpression LowerPseudoInverse(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.LowerPseudoInverse(expressionName, settings);

    public static CurveExpression LowerPseudoInverse(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new LowerPseudoInverseExpression(curve, name, expressionName, settings);

    public static CurveExpression UpperPseudoInverse(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.UpperPseudoInverse(expressionName, settings);

    public static CurveExpression UpperPseudoInverse(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new UpperPseudoInverseExpression(curve, name, expressionName, settings);

    public static CurveExpression Addition(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Addition(curve, name, expressionName, settings);

    public static CurveExpression Addition(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Addition(expressionR, expressionName, settings);

    public static CurveExpression Addition(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new AdditionExpression([curveL, curveR], [nameL, nameR], expressionName, settings);

    public static CurveExpression Addition(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromCurve(curveL, nameL).Addition(expressionR, expressionName, settings);

    public static CurveExpression Subtraction(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Subtraction(expressionR, expressionName, settings);

    public static CurveExpression Subtraction(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Subtraction(curve, name, expressionName, settings);

    public static CurveExpression Subtraction(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new SubtractionExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static CurveExpression Subtraction(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new SubtractionExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression Minimum(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Minimum(expressionR, expressionName, settings);

    public static CurveExpression Minimum(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Minimum(curve, name, expressionName, settings);

    public static CurveExpression Minimum(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new MinimumExpression([curveL, curveR], [nameL, nameR], expressionName, settings);

    public static CurveExpression Minimum(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromCurve(curveL, nameL).Minimum(expressionR, expressionName, settings);

    public static CurveExpression Maximum(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Maximum(expressionR, expressionName, settings);

    public static CurveExpression Maximum(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Maximum(curve, name, expressionName, settings);

    public static CurveExpression Maximum(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new MaximumExpression([curveL, curveR], [nameL, nameR], expressionName, settings);

    public static CurveExpression Maximum(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromCurve(curveL, nameL).Maximum(expressionR, expressionName, settings);

    public static CurveExpression Convolution(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Convolution(expressionR, expressionName, settings);

    public static CurveExpression Convolution(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Convolution(curve, name, expressionName, settings);

    public static CurveExpression Convolution(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ConvolutionExpression([curveL, curveR], [nameL, nameR], expressionName, settings);

    public static CurveExpression Convolution(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromCurve(curveL, nameL).Convolution(expressionR, expressionName, settings);

    public static CurveExpression Deconvolution(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Deconvolution(expressionR, expressionName, settings);

    public static CurveExpression Deconvolution(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Deconvolution(curve, name, expressionName, settings);

    public static CurveExpression Deconvolution(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new DeconvolutionExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static CurveExpression Deconvolution(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new DeconvolutionExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression MaxPlusConvolution(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.MaxPlusConvolution(expressionR, expressionName, settings);

    public static CurveExpression MaxPlusConvolution(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.MaxPlusConvolution(curve, name, expressionName, settings);

    public static CurveExpression MaxPlusConvolution(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new MaxPlusConvolutionExpression([curveL, curveR], [nameL, nameR], expressionName, settings);

    public static CurveExpression MaxPlusConvolution(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromCurve(curveL, nameL).MaxPlusConvolution(expressionR, expressionName, settings);

    public static CurveExpression MaxPlusDeconvolution(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.MaxPlusDeconvolution(expressionR, expressionName, settings);

    public static CurveExpression MaxPlusDeconvolution(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.MaxPlusDeconvolution(curve, name, expressionName, settings);

    public static CurveExpression MaxPlusDeconvolution(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new MaxPlusDeconvolutionExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static CurveExpression MaxPlusDeconvolution(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new MaxPlusDeconvolutionExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression Composition(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Composition(expressionR, expressionName, settings);

    public static CurveExpression Composition(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Composition(curve, name, expressionName, settings);

    public static CurveExpression Composition(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new CompositionExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static CurveExpression Composition(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new CompositionExpression(curveL, nameL, expressionR, expressionName, settings);

    public static RationalExpression HorizontalDeviation(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => new HorizontalDeviationExpression(expressionL, expressionR, expressionName, settings);

    public static RationalExpression HorizontalDeviation(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new HorizontalDeviationExpression(expression, new ConcreteCurveExpression(curve, name), expressionName, settings);

    public static RationalExpression HorizontalDeviation(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new HorizontalDeviationExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static RationalExpression HorizontalDeviation(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new HorizontalDeviationExpression(curveL, nameL, expressionR, expressionName, settings);

    public static RationalExpression VerticalDeviation(CurveExpression expressionL, CurveExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => new VerticalDeviationExpression(expressionL, expressionR, expressionName, settings);

    public static RationalExpression VerticalDeviation(CurveExpression expression, Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new VerticalDeviationExpression(expression, new ConcreteCurveExpression(curve, name), expressionName, settings);

    public static RationalExpression VerticalDeviation(Curve curveL, Curve curveR,
        [CallerArgumentExpression("curveL")] string nameL = "", [CallerArgumentExpression("curveR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new VerticalDeviationExpression(curveL, nameL, curveR, nameR, expressionName, settings);

    public static RationalExpression VerticalDeviation(Curve curveL, CurveExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new VerticalDeviationExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression DelayBy(CurveExpression expressionL, RationalExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.DelayBy(expressionR, expressionName, settings);

    public static CurveExpression DelayBy(CurveExpression expression, Rational delay,
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.DelayBy(delay, expressionName, settings);

    public static CurveExpression DelayBy(Curve curveL, Rational delay,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new DelayByExpression(curveL, nameL, delay, expressionName, settings);

    public static CurveExpression DelayBy(Curve curveL, RationalExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new DelayByExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression AnticipateBy(CurveExpression expressionL, RationalExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.AnticipateBy(expressionR, expressionName, settings);

    public static CurveExpression AnticipateBy(CurveExpression expression, Rational time,
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.AnticipateBy(time, expressionName, settings);

    public static CurveExpression AnticipateBy(Curve curveL, Rational time,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new AnticipateByExpression(curveL, nameL, time, expressionName, settings);

    public static CurveExpression AnticipateBy(Curve curveL, RationalExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new AnticipateByExpression(curveL, nameL, expressionR, expressionName, settings);

    public static CurveExpression Scale(CurveExpression expressionL, RationalExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Scale(expressionR, expressionName, settings);

    public static CurveExpression Scale(CurveExpression expression, Rational time,
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Scale(time, expressionName, settings);

    public static CurveExpression Scale(Curve curveL, Rational time,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ScaleExpression(curveL, nameL, time, expressionName, settings);

    public static CurveExpression Scale(Curve curveL, RationalExpression expressionR,
        [CallerArgumentExpression("curveL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new ScaleExpression(curveL, nameL, expressionR, expressionName, settings);
    
    public static RationalExpression RationalAddition(RationalExpression expression, Rational number,
        [CallerArgumentExpression("number")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => expression.Addition(number, name, expressionName, settings);

    public static RationalExpression RationalAddition(RationalExpression expressionL, RationalExpression expressionR,
        string expressionName = "", ExpressionSettings? settings = null)
        => expressionL.Addition(expressionR, expressionName, settings);

    public static RationalExpression RationalAddition(Rational rationalL, Rational rationalR,
        [CallerArgumentExpression("rationalL")] string nameL = "", [CallerArgumentExpression("rationalR")] string nameR = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => new RationalAdditionExpression([rationalL, rationalR], [nameL, nameR], expressionName, settings);

    public static RationalExpression RationalAddition(Rational rationalL, RationalExpression expressionR,
        [CallerArgumentExpression("rationalL")] string nameL = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => FromRational(rationalL, nameL).Addition(expressionR, expressionName, settings);
    
    public static CurveExpression Addition(IReadOnlyCollection<Curve> curves, IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null)
        => new AdditionExpression(curves, names, expressionName, settings);
    
    public static RationalExpression RationalAddition(IReadOnlyCollection<Rational> numbers, IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null)
        => new RationalAdditionExpression(numbers, names, expressionName, settings);

    public static CurveExpression Minimum(IReadOnlyCollection<Curve> curves, IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null)
        => new MinimumExpression(curves, names, expressionName, settings);

    public static CurveExpression Maximum(IReadOnlyCollection<Curve> curves, IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null)
        => new MaximumExpression(curves, names, expressionName, settings);

    public static CurveExpression Convolution(IReadOnlyCollection<Curve> curves, IReadOnlyCollection<string> names,
        string expressionName = "", ExpressionSettings? settings = null)
        => new ConvolutionExpression(curves, names, expressionName, settings);

    public static CurveExpression MaxPlusConvolution(IReadOnlyCollection<Curve> curves,
        IReadOnlyCollection<string> names, string expressionName = "", ExpressionSettings? settings = null)
        => new MaxPlusConvolutionExpression(curves, names, expressionName, settings);

    public static RationalExpression Negate(RationalExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.Negate(expressionName, settings);

    public static RationalExpression Negate(Rational number, string expressionName = "", ExpressionSettings? settings = null)
        => new NegateRationalExpression(number, expressionName, settings);

    public static RationalExpression Invert(RationalExpression expression, string expressionName = "", ExpressionSettings? settings = null)
        => expression.Invert(expressionName, settings);

    public static RationalExpression Invert(Rational number, string expressionName = "", ExpressionSettings? settings = null)
        => new InvertRationalExpression(number, expressionName, settings);

    public static CurveExpression Placeholder(string name, ExpressionSettings? settings = null)
        => new CurvePlaceholderExpression(name, settings);
    
    public static RationalExpression RationalPlaceholder(string name, ExpressionSettings? settings = null)
        => new RationalPlaceholderExpression(name, settings);

    public static T Compute<T>(IGenericExpression<T> expression)
        => expression.Compute();

    public static bool Equivalent(CurveExpression e1, CurveExpression e2)
        => e1.Equivalent(e2);

    public static bool CompareHorizontalDeviation(CurveExpression aExpr, CurveExpression b1, CurveExpression b1Other)
        => CompareHorizontalDeviation(aExpr.Compute(), b1, b1Other);

    public static bool CompareHorizontalDeviation(Curve a, CurveExpression b1, CurveExpression b1Other)
        => Curve.HorizontalDeviation(a, b1.Compute()) ==
           Curve.HorizontalDeviation(a, b1Other.Compute());
}