using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.ExpressionsUtility;
using Unipi.Nancy.Expressions.ExpressionsUtility.Internals;
using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions;

/// <summary>
/// Class which describes NetCal expressions that evaluate to curves. The class aims at providing the main methods to
/// build, manipulate and print network calculus expressions.
/// </summary>
/// <param name="expressionName">The name of the expression</param>
/// <param name="settings"></param>
public abstract class CurveExpression(string expressionName = "", ExpressionSettings? settings = null)
    : IGenericExpression<Curve>, IVisitableCurve
{
    public string Name { get; } = expressionName;

    public static readonly ConcurrentDictionary<Type, List<Equivalence>> Equivalences = new();

    public ExpressionSettings? Settings { get; } = settings;

    public CurveExpression Negate(string expressionName = "", ExpressionSettings? settings = null)
        => new NegateExpression(this, expressionName, settings);

    public CurveExpression ToNonNegative(string expressionName = "", ExpressionSettings? settings = null)
        => new ToNonNegativeExpression(this, expressionName, settings);

    public CurveExpression SubAdditiveClosure(string expressionName = "", ExpressionSettings? settings = null)
        => new SubAdditiveClosureExpression(this, expressionName, settings);

    public CurveExpression SuperAdditiveClosure(string expressionName = "", ExpressionSettings? settings = null)
        => new SuperAdditiveClosureExpression(this, expressionName, settings);

    public CurveExpression ToUpperNonDecreasing(string expressionName = "", ExpressionSettings? settings = null)
        => new ToUpperNonDecreasingExpression(this, expressionName, settings);

    public CurveExpression ToLowerNonDecreasing(string expressionName = "", ExpressionSettings? settings = null)
        => new ToLowerNonDecreasingExpression(this, expressionName, settings);

    public CurveExpression ToLeftContinuous(string expressionName = "", ExpressionSettings? settings = null)
        => new ToLeftContinuousExpression(this, expressionName, settings);

    public CurveExpression ToRightContinuous(string expressionName = "", ExpressionSettings? settings = null)
        => new ToRightContinuousExpression(this, expressionName, settings);

    public CurveExpression WithZeroOrigin(string expressionName = "", ExpressionSettings? settings = null)
        => new WithZeroOriginExpression(this, expressionName, settings);

    public CurveExpression LowerPseudoInverse(string expressionName = "", ExpressionSettings? settings = null)
        => new LowerPseudoInverseExpression(this, expressionName, settings);

    public CurveExpression UpperPseudoInverse(string expressionName = "", ExpressionSettings? settings = null)
        => new UpperPseudoInverseExpression(this, expressionName, settings);

    public CurveExpression Addition(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(AdditionExpression), this, expression) switch
        {
            1 => ((AdditionExpression)this).Append(expression, expressionName, settings),
            2 => ((AdditionExpression)expression).Append(this, expressionName, settings),
            _ => new AdditionExpression([this, expression], expressionName, settings)
        };

    public CurveExpression Addition(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is AdditionExpression e)
            return e.Append(new ConcreteCurveExpression(curve, name), expressionName, settings);
        return new AdditionExpression([this, new ConcreteCurveExpression(curve, name)], expressionName, settings);
    }

    public static CurveExpression Addition(CurveExpression left, Curve right, string expressionName = "",
        ExpressionSettings? settings = null)
        => left.Addition(right, settings: settings);

    public static CurveExpression operator +(CurveExpression left, Curve right)
        => Addition(left, right);

    public CurveExpression Subtraction(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new SubtractionExpression(this, expression, expressionName, settings);

    public CurveExpression Subtraction(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => Subtraction(new ConcreteCurveExpression(curve, name), expressionName, settings);

    public CurveExpression Minimum(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(MinimumExpression), this, expression) switch
        {
            1 => ((MinimumExpression)this).Append(expression, expressionName, settings),
            2 => ((MinimumExpression)expression).Append(this, expressionName, settings),
            _ => new MinimumExpression([this, expression], expressionName, settings)
        };

    public CurveExpression Minimum(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is MinimumExpression e)
            return e.Append(new ConcreteCurveExpression(curve, name), expressionName, settings);
        return new MinimumExpression([this, new ConcreteCurveExpression(curve, name)], expressionName, settings);
    }

    public CurveExpression Maximum(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(MaximumExpression), this, expression) switch
        {
            1 => ((MaximumExpression)this).Append(expression, expressionName, settings),
            2 => ((MaximumExpression)expression).Append(this, expressionName, settings),
            _ => new MaximumExpression([this, expression], expressionName, settings),
        };

    public CurveExpression Maximum(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is MaximumExpression e)
            return e.Append(new ConcreteCurveExpression(curve, name, settings));
        return new MaximumExpression([this, new ConcreteCurveExpression(curve, name)], expressionName, settings);
    }

    public CurveExpression Convolution(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(ConvolutionExpression), this, expression) switch
        {
            1 => ((ConvolutionExpression)this).Append(expression, expressionName, settings),
            2 => ((ConvolutionExpression)expression).Append(this, expressionName, settings),
            _ => new ConvolutionExpression([this, expression], expressionName, settings),
        };

    public CurveExpression Convolution(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is ConvolutionExpression e)
            return e.Append(new ConcreteCurveExpression(curve, name), expressionName, settings);
        return new ConvolutionExpression([this, new ConcreteCurveExpression(curve, name)], expressionName, settings);
    }

    public CurveExpression Deconvolution(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new DeconvolutionExpression(this, expression, expressionName, settings);

    public CurveExpression Deconvolution(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => Deconvolution(new ConcreteCurveExpression(curve, name), expressionName, settings);

    public CurveExpression MaxPlusConvolution(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(MaxPlusConvolutionExpression), this, expression) switch
        {
            1 => ((MaxPlusConvolutionExpression)this).Append(expression, expressionName, settings),
            2 => ((MaxPlusConvolutionExpression)expression).Append(this, expressionName, settings),
            _ => new MaxPlusConvolutionExpression([this, expression], expressionName, settings),
        };

    public CurveExpression MaxPlusConvolution(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is MaxPlusConvolutionExpression e)
            return e.Append(new ConcreteCurveExpression(curve, name), expressionName, settings);
        return new MaxPlusConvolutionExpression([this, new ConcreteCurveExpression(curve, name)], expressionName,
            settings);
    }

    public CurveExpression MaxPlusDeconvolution(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new MaxPlusDeconvolutionExpression(this, expression, expressionName, settings);

    public CurveExpression MaxPlusDeconvolution(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => MaxPlusDeconvolution(new ConcreteCurveExpression(curve, name), expressionName, settings);

    public CurveExpression Composition(CurveExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new CompositionExpression(this, expression, expressionName, settings);

    public CurveExpression Composition(Curve curve, [CallerArgumentExpression("curve")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
        => Composition(new ConcreteCurveExpression(curve, name), expressionName, settings);

    public CurveExpression DelayBy(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new DelayByExpression(this, expression, expressionName, settings);

    public CurveExpression DelayBy(Rational delay, string expressionName = "", ExpressionSettings? settings = null)
        => DelayBy(new RationalNumberExpression(delay), expressionName, settings);

    public CurveExpression AnticipateBy(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new DelayByExpression(this, expression, expressionName, settings);

    public CurveExpression AnticipateBy(Rational time, string expressionName = "", ExpressionSettings? settings = null)
        => AnticipateBy(new RationalNumberExpression(time), expressionName, settings);

    public CurveExpression Scale(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new ScaleExpression(this, expression, expressionName, settings);

    public CurveExpression Scale(Rational delay, string expressionName = "", ExpressionSettings? settings = null)
        => Scale(new RationalNumberExpression(delay), expressionName, settings);

    /// <summary>
    /// Private cache field for <see cref="Value"/>
    /// </summary>
    internal Curve? _value;

    public Curve Value => _value ??= Compute();

    public Curve Compute() => _value ??= new CurveExpressionEvaluator().GetResult(this);

    public CurveExpression ReplaceByValue<T1>(IGenericExpression<T1> expressionPattern,
        IGenericExpression<T1> newExpressionToReplace)
    {
        var replacer = new ExpressionReplacer<Curve, T1>(this, newExpressionToReplace);
        return (CurveExpression)replacer.ReplaceByValue(expressionPattern);
    }

    IGenericExpression<Curve> IGenericExpression<Curve>.ReplaceByValue<T1>(IGenericExpression<T1> expressionPattern,
        IGenericExpression<T1> newExpressionToReplace) => ReplaceByValue(expressionPattern, newExpressionToReplace);

    public CurveExpression ReplaceByPosition<T1>(ExpressionPosition expressionPosition,
        IGenericExpression<T1> newExpressionToReplace)
        => ReplaceByPosition(expressionPosition.GetPositionPath(), newExpressionToReplace);

    IGenericExpression<Curve> IGenericExpression<Curve>.ReplaceByPosition<T1>(ExpressionPosition expressionPosition,
        IGenericExpression<T1> newExpressionToReplace) => ReplaceByPosition(expressionPosition, newExpressionToReplace);

    public CurveExpression ReplaceByPosition<T1>(IEnumerable<string> positionPath,
        IGenericExpression<T1> newExpressionToReplace)
    {
        var replacer = new ExpressionReplacer<Curve, T1>(this, newExpressionToReplace);
        return (CurveExpression)replacer.ReplaceByPosition(positionPath);
    }

    IGenericExpression<Curve> IGenericExpression<Curve>.ReplaceByPosition<T1>(IEnumerable<string> positionPath,
        IGenericExpression<T1> newExpressionToReplace) => ReplaceByPosition(positionPath, newExpressionToReplace);

    public CurveExpression ApplyEquivalence(Equivalence equivalence, CheckType checkType = CheckType.CheckLeftOnly)
    {
        var replacer = new ExpressionReplacer<Curve, Curve>(this, equivalence, checkType);
        // In the case of equivalences the argument of ReplaceByValue is not significant
        return (CurveExpression)replacer.ReplaceByValue(equivalence.LeftSideExpression);
    }

    IGenericExpression<Curve> IGenericExpression<Curve>.ApplyEquivalence(Equivalence equivalence, CheckType checkType)
        => ApplyEquivalence(equivalence, checkType);

    public CurveExpression ApplyEquivalenceByPosition(IEnumerable<string> positionPath, Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly)
    {
        var replacer = new ExpressionReplacer<Curve, Curve>(this, equivalence, checkType);
        return (CurveExpression)replacer.ReplaceByPosition(positionPath);
    }

    IGenericExpression<Curve> IGenericExpression<Curve>.ApplyEquivalenceByPosition(IEnumerable<string> positionPath,
        Equivalence equivalence,
        CheckType checkType)
        => ApplyEquivalenceByPosition(positionPath, equivalence, checkType);

    public CurveExpression ApplyEquivalenceByPosition(ExpressionPosition expressionPosition, Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly)
        => ApplyEquivalenceByPosition(expressionPosition.GetPositionPath(), equivalence, checkType);

    IGenericExpression<Curve> IGenericExpression<Curve>.ApplyEquivalenceByPosition(
        ExpressionPosition expressionPosition, Equivalence equivalence,
        CheckType checkType)
        => ApplyEquivalenceByPosition(expressionPosition, equivalence, checkType);

    public void Accept(IExpressionVisitor visitor)
        => Accept((ICurveExpressionVisitor)visitor);

    public abstract void Accept(ICurveExpressionVisitor visitor);

    /// <summary>
    /// Checks if two expressions are equivalent by computing their values
    /// </summary>
    public bool Equivalent(IGenericExpression<Curve> other)
        => Curve.Equivalent(Compute(),
            other.Compute());


    private static int CheckNAryExpressionTypes(Type type, CurveExpression e1, CurveExpression e2)
    {
        if (e1.GetType() == type)
            return 1;
        return e2.GetType() == type ? 2 : 0;
    }

    public string ToLatexString(int depth = 20, bool showRationalsAsName = false)
    {
        var latexFormatterVisitor = new LatexFormatterVisitor(depth, showRationalsAsName);
        Accept(latexFormatterVisitor);

        var latexExpr = latexFormatterVisitor.Result.ToString();
        if (latexExpr is ['(', _, ..] && latexExpr[^1] == ')') // ^1 accesses the last character
            latexExpr = latexExpr[1..^1];

        return latexExpr;
    }

    public string ToUnicodeString(int depth = 20, bool showRationalsAsName = false)
    {
        var unicodeFormatterVisitor = new UnicodeFormatterVisitor(depth, showRationalsAsName);
        Accept(unicodeFormatterVisitor);

        var unicodeExpr = unicodeFormatterVisitor.Result.ToString();
        if (unicodeExpr is ['(', _, ..] && unicodeExpr[^1] == ')') // ^1 accesses the last character
            unicodeExpr = unicodeExpr[1..^1];

        return unicodeExpr;
    }

    public override string ToString()
        => ToUnicodeString();

    public double Estimate()
    {
        throw new NotImplementedException();
    }
    
    public static void AddEquivalence(Type type, Equivalence equivalence)
    {
        if (equivalence.LeftSideExpression.GetType() != type) return;
        Equivalences.TryAdd(type, []);
        Equivalences[type].Add(equivalence);
    }

    public ExpressionPosition RootPosition() => new();

    public CurveExpression WithName(string expressionName)
    {
        var changeNameVisitor = new ChangeNameCurveVisitor(expressionName);
        Accept(changeNameVisitor);

        return changeNameVisitor.Result;
    }

    IGenericExpression<Curve> IGenericExpression<Curve>.WithName(string expressionName) => WithName(expressionName);

    public static bool operator <=(CurveExpression expressionL, CurveExpression expressionR)
        => expressionL.Compute() <= expressionR.Compute();

    public static bool operator >=(CurveExpression expressionL, CurveExpression expressionR)
        => expressionL.Compute() >= expressionR.Compute();

    // Cut-Through properties
    internal bool? _isSubAdditive;

    public bool IsSubAdditive
    {
        get
        {
            return _isSubAdditive ??= CheckIsSubAdditive();

            bool CheckIsSubAdditive()
            {
                var isSubAdditiveVisitor = new IsSubAdditiveVisitor();
                Accept(isSubAdditiveVisitor);

                return isSubAdditiveVisitor.IsSubAdditive;
            }
        }
    }

    internal bool? _isLeftContinuous;

    public bool IsLeftContinuous
    {
        get
        {
            return _isLeftContinuous ??= CheckIsLeftContinuous();

            bool CheckIsLeftContinuous()
            {
                var isLeftContinuousVisitor = new IsLeftContinuousVisitor();
                Accept(isLeftContinuousVisitor);

                return isLeftContinuousVisitor.IsLeftContinuous;
            }
        }
    }

    internal bool? _isRightContinuous;

    public bool IsRightContinuous
    {
        get
        {
            return _isRightContinuous ??= CheckIsRightContinuous();

            bool CheckIsRightContinuous()
            {
                var isRightContinuousVisitor = new IsRightContinuousVisitor();
                Accept(isRightContinuousVisitor);

                return isRightContinuousVisitor.IsRightContinuous;
            }
        }
    }

    internal bool? _isNonNegative;

    public bool IsNonNegative
    {
        get
        {
            return _isNonNegative ??= CheckIsNonNegative();

            bool CheckIsNonNegative()
            {
                var isNonNegativeVisitor = new IsNonNegativeVisitor();
                Accept(isNonNegativeVisitor);

                return isNonNegativeVisitor.IsNonNegative;
            }
        }
    }

    internal bool? _isNonDecreasing;

    public bool IsNonDecreasing
    {
        get
        {
            return _isNonDecreasing ??= CheckIsNonDecreasing();

            bool CheckIsNonDecreasing()
            {
                var isNonDecreasingVisitor = new IsNonDecreasingVisitor();
                Accept(isNonDecreasingVisitor);

                return isNonDecreasingVisitor.IsNonDecreasing;
            }
        }
    }

    internal bool? _isConcave;

    public bool IsConcave
    {
        get
        {
            return _isConcave ??= CheckIsConcave();

            bool CheckIsConcave()
            {
                var isConcaveVisitor = new IsConcaveVisitor();
                Accept(isConcaveVisitor);

                return isConcaveVisitor.IsConcave;
            }
        }
    }

    internal bool? _isConvex;

    public bool IsConvex
    {
        get
        {
            return _isConvex ??= CheckIsConvex();

            bool CheckIsConvex()
            {
                var isConvexVisitor = new IsConvexVisitor();
                Accept(isConvexVisitor);

                return isConvexVisitor.IsConvex;
            }
        }
    }

    internal bool? _isZeroAtZero;

    public bool IsZeroAtZero
    {
        get
        {
            return _isZeroAtZero ??= CheckIsZeroAtZero();

            bool CheckIsZeroAtZero()
            {
                var isZeroAtZeroVisitor = new IsZeroAtZeroVisitor();
                Accept(isZeroAtZeroVisitor);

                return isZeroAtZeroVisitor.IsZeroAtZero;
            }
        }
    }

    internal bool? _isWellDefined;

    public bool IsWellDefined
    {
        get
        {
            return _isWellDefined ??= CheckIsWellDefined();

            bool CheckIsWellDefined()
            {
                var isWellDefinedVisitor = new IsWellDefinedVisitor();
                Accept(isWellDefinedVisitor);

                return isWellDefinedVisitor.IsWellDefined;
            }
        }
    }

    public bool IsUltimatelyConstant() => Compute().IsUltimatelyConstant;
}