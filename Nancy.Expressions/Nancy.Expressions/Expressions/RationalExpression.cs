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
/// The class aims at providing the main methods to build, manipulate and print expressions which evaluate to rational
/// numbers and are based on NetCal curves.
/// </summary>
/// <param name="expressionName">The name of the expression</param>
/// <param name="settings"></param>
public abstract class RationalExpression(string expressionName = "", ExpressionSettings? settings = null)
    : IGenericExpression<Rational>, IVisitableRational
{
    public string Name { get; } = expressionName;

    public ExpressionSettings? Settings { get; } = settings;

    public RationalExpression Negate(string expressionName = "", ExpressionSettings? settings = null)
        => new NegateRationalExpression(this, expressionName, settings);

    public RationalExpression Invert(string expressionName = "", ExpressionSettings? settings = null)
        => new InvertRationalExpression(this, expressionName, settings);

    internal Rational? _value;

    public Rational Value => _value ??= Compute();

    public Rational Compute() => _value ??= new RationalExpressionEvaluator().GetResult(this);

    public void Accept(IExpressionVisitor visitor)
        => Accept((IRationalExpressionVisitor)visitor);

    public abstract void Accept(IRationalExpressionVisitor visitor);

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

    public RationalExpression ReplaceByValue<T1>(IGenericExpression<T1> expressionPattern,
        IGenericExpression<T1> newExpressionToReplace)
    {
        var replacer = new ExpressionReplacer<Rational, T1>(this, newExpressionToReplace);
        return (RationalExpression)replacer.ReplaceByValue(expressionPattern);
    }

    IGenericExpression<Rational> IGenericExpression<Rational>.ReplaceByValue<T1>(
        IGenericExpression<T1> expressionPattern,
        IGenericExpression<T1> newExpressionToReplace) => ReplaceByValue(expressionPattern, newExpressionToReplace);

    public RationalExpression ReplaceByPosition<T1>(ExpressionPosition expressionPosition,
        IGenericExpression<T1> newExpressionToReplace)
        => ReplaceByPosition(expressionPosition.GetPositionPath(), newExpressionToReplace);

    IGenericExpression<Rational> IGenericExpression<Rational>.ReplaceByPosition<T1>(
        ExpressionPosition expressionPosition, IGenericExpression<T1> newExpressionToReplace) =>
        ReplaceByPosition(expressionPosition, newExpressionToReplace);

    public RationalExpression ReplaceByPosition<T1>(IEnumerable<string> positionPath,
        IGenericExpression<T1> newExpressionToReplace)
    {
        var replacer = new ExpressionReplacer<Rational, T1>(this, newExpressionToReplace);
        return (RationalExpression)replacer.ReplaceByPosition(positionPath);
    }

    IGenericExpression<Rational> IGenericExpression<Rational>.ReplaceByPosition<T1>(IEnumerable<string> positionPath,
        IGenericExpression<T1> newExpressionToReplace) => ReplaceByPosition(positionPath, newExpressionToReplace);

    public ExpressionPosition RootPosition() => new();

    public RationalExpression WithName(string expressionName)
    {
        var changeNameVisitor = new ChangeNameRationalVisitor(expressionName);
        Accept(changeNameVisitor);

        return changeNameVisitor.Result;
    }

    IGenericExpression<Rational> IGenericExpression<Rational>.WithName(string expressionName) =>
        WithName(expressionName);
    
    public static bool operator <=(RationalExpression expressionL, RationalExpression expressionR)
        => expressionL.Compute() <= expressionR.Compute();

    public static bool operator >=(RationalExpression expressionL, RationalExpression expressionR)
        => expressionL.Compute() >= expressionR.Compute();

    #region Addition
    
    public RationalExpression Addition(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(RationalAdditionExpression), this, expression) switch
        {
            1 => ((RationalAdditionExpression)this).Append(expression, expressionName, settings),
            2 => ((RationalAdditionExpression)expression).Append(this, expressionName, settings),
            _ => new RationalAdditionExpression([this, expression], expressionName, settings),
        };

    public static RationalExpression Addition(RationalExpression left, RationalExpression right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.Addition(right);

    public static RationalExpression operator +(RationalExpression left, RationalExpression right)
        => Addition(left, right);

    public RationalExpression Addition(Rational rational, [CallerArgumentExpression("rational")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is RationalAdditionExpression e)
            return e.Append(new RationalNumberExpression(rational, name), expressionName, settings);
        return new RationalAdditionExpression([this, new RationalNumberExpression(rational, name)], expressionName,
            settings);
    }

    public static RationalExpression Addition(RationalExpression left, Rational right, string expressionName = "",
        ExpressionSettings? settings = null)
        => left.Addition(right);

    public static RationalExpression operator +(RationalExpression left, Rational right)
        => Addition(left, right);

    #endregion Addition
    
    // todo: add subtraction
    
    #region Product
    
    public RationalExpression Product(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(RationalProductExpression), this, expression) switch
        {
            1 => ((RationalProductExpression)this).Append(expression, expressionName, settings),
            2 => ((RationalProductExpression)expression).Append(this, expressionName, settings),
            _ => new RationalProductExpression([this, expression], expressionName, settings),
        };

    public static RationalExpression Product(RationalExpression left, RationalExpression right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.Product(right);

    public static RationalExpression operator *(RationalExpression left, RationalExpression right)
        => Product(left, right);

    public RationalExpression Product(Rational rational, [CallerArgumentExpression("rational")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is RationalProductExpression e)
            return e.Append(new RationalNumberExpression(rational, name), expressionName, settings);
        return new RationalProductExpression([this, new RationalNumberExpression(rational, name)], expressionName,
            settings);
    }

    public static RationalExpression Product(RationalExpression left, Rational right, string expressionName = "",
        ExpressionSettings? settings = null)
        => left.Product(right);

    public static RationalExpression operator *(RationalExpression left, Rational right)
        => Product(left, right);
    
    #endregion Product
    
    #region Division
    
    public RationalExpression Division(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => new RationalDivisionExpression(this, expression, expressionName, settings);

    public static RationalExpression Division(RationalExpression left, RationalExpression right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.Division(right);

    public static RationalExpression operator /(RationalExpression left, RationalExpression right)
        => Division(left, right);

    public RationalExpression Division(Rational rational, [CallerArgumentExpression("rational")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        return new RationalDivisionExpression(
            this, new RationalNumberExpression(rational, name), 
            expressionName, settings
        );
    }

    public static RationalExpression Division(RationalExpression left, Rational right, string expressionName = "",
        ExpressionSettings? settings = null)
        => left.Division(right);

    public static RationalExpression operator /(RationalExpression left, Rational right)
        => Division(left, right);
    
    #endregion Division
    
    #region LeastCommonMultiple
    
    public RationalExpression LeastCommonMultiple(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(RationalLeastCommonMultipleExpression), this, expression) switch
        {
            1 => ((RationalLeastCommonMultipleExpression)this).Append(expression, expressionName, settings),
            2 => ((RationalLeastCommonMultipleExpression)expression).Append(this, expressionName, settings),
            _ => new RationalLeastCommonMultipleExpression([this, expression], expressionName, settings),
        };
    
    public static RationalExpression LeastCommonMultiple(RationalExpression left, RationalExpression right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.LeastCommonMultiple(right);
    
    public RationalExpression LeastCommonMultiple(Rational rational,
        [CallerArgumentExpression("rational")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is RationalLeastCommonMultipleExpression e)
            return e.Append(new RationalNumberExpression(rational, name), expressionName, settings);
        return new RationalLeastCommonMultipleExpression([this, new RationalNumberExpression(rational, name)], expressionName,
            settings);
    }

    public static RationalExpression LeastCommonMultiple(RationalExpression left, Rational right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.LeastCommonMultiple(right);
    
    #endregion LeastCommonMultiple
    
    #region GreatestCommonDivisor
    
    public RationalExpression GreatestCommonDivisor(RationalExpression expression, string expressionName = "",
        ExpressionSettings? settings = null)
        => CheckNAryExpressionTypes(typeof(RationalGreatestCommonDivisorExpression), this, expression) switch
        {
            1 => ((RationalGreatestCommonDivisorExpression)this).Append(expression, expressionName, settings),
            2 => ((RationalGreatestCommonDivisorExpression)expression).Append(this, expressionName, settings),
            _ => new RationalGreatestCommonDivisorExpression([this, expression], expressionName, settings),
        };
    
    public static RationalExpression GreatestCommonDivisor(RationalExpression left, RationalExpression right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.GreatestCommonDivisor(right);
    
    public RationalExpression GreatestCommonDivisor(Rational rational,
        [CallerArgumentExpression("rational")] string name = "",
        string expressionName = "", ExpressionSettings? settings = null)
    {
        if (this is RationalGreatestCommonDivisorExpression e)
            return e.Append(new RationalNumberExpression(rational, name), expressionName, settings);
        return new RationalGreatestCommonDivisorExpression([this, new RationalNumberExpression(rational, name)], expressionName,
            settings);
    }

    public static RationalExpression GreatestCommonDivisor(RationalExpression left, Rational right,
        string expressionName = "", ExpressionSettings? settings = null)
        => left.GreatestCommonDivisor(right);
    
    #endregion GreatestCommonDivisor
    
    private static int CheckNAryExpressionTypes(Type type, RationalExpression e1, RationalExpression e2)
    {
        if (e1.GetType() == type)
            return 1;
        return e2.GetType() == type ? 2 : 0;
    }
    
    public RationalExpression ApplyEquivalence(Equivalence equivalence, CheckType checkType = CheckType.CheckLeftOnly)
    {
        var replacer = new ExpressionReplacer<Rational, Curve>(this, equivalence, checkType);
        // In the case of equivalences the argument of ReplaceByValue is not significant
        return (RationalExpression)replacer.ReplaceByValue(equivalence.LeftSideExpression);
    }

    IGenericExpression<Rational> IGenericExpression<Rational>.ApplyEquivalence(Equivalence equivalence,
        CheckType checkType)
        => ApplyEquivalence(equivalence, checkType);

    public RationalExpression ApplyEquivalenceByPosition(IEnumerable<string> positionPath, Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly)
    {
        var replacer = new ExpressionReplacer<Rational, Curve>(this, equivalence, checkType);
        return (RationalExpression)replacer.ReplaceByPosition(positionPath);
    }

    IGenericExpression<Rational> IGenericExpression<Rational>.ApplyEquivalenceByPosition(
        IEnumerable<string> positionPath, Equivalence equivalence,
        CheckType checkType)
        => ApplyEquivalenceByPosition(positionPath, equivalence, checkType);

    public RationalExpression ApplyEquivalenceByPosition(ExpressionPosition expressionPosition, Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly)
        => ApplyEquivalenceByPosition(expressionPosition.GetPositionPath(), equivalence, checkType);
    
    IGenericExpression<Rational> IGenericExpression<Rational>.ApplyEquivalenceByPosition(ExpressionPosition expressionPosition, Equivalence equivalence,
        CheckType checkType)
        => ApplyEquivalenceByPosition(expressionPosition, equivalence, checkType);
}