using Unipi.Nancy.Expressions.Internals;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Visitors;

/// <summary>
/// Visitor class used to update the name of a rational expression.
/// </summary>
/// <param name="newName">The new name of the expression</param>
public class ChangeNameRationalVisitor(string newName) : IRationalExpressionVisitor
{
    public RationalExpression Result = Expressions.FromRational(Rational.Zero);

    public void Visit(HorizontalDeviationExpression expression)
        => Result = Expressions.HorizontalDeviation((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(VerticalDeviationExpression expression)
        => Result = Expressions.VerticalDeviation((CurveExpression)expression.LeftExpression,
            (CurveExpression)expression.RightExpression, newName);

    public void Visit(RationalAdditionExpression expression)
        => Result = new RationalAdditionExpression(expression.Expressions, newName);

    public void Visit(RationalProductExpression expression)
        => Result = new RationalProductExpression(expression.Expressions, newName);

    public void Visit(RationalDivisionExpression expression)
        => Result = new RationalDivisionExpression(expression.LeftExpression, expression.RightExpression, newName);

    public void Visit(RationalLeastCommonMultipleExpression expression)
        => Result = new RationalLeastCommonMultipleExpression(expression.Expressions, newName);

    public void Visit(RationalGreatestCommonDivisorExpression expression)
        => Result = new RationalGreatestCommonDivisorExpression(expression.Expressions, newName);

    public void Visit(RationalNumberExpression expression)
        => Result = Expressions.FromRational(expression.Value, newName);

    public void Visit(NegateRationalExpression expression)
        => Result = Expressions.Negate((RationalExpression)expression.Expression, newName);

    public void Visit(InvertRationalExpression expression)
        => Result = Expressions.Invert((RationalExpression)expression.Expression, newName);

    public void Visit(RationalPlaceholderExpression expression)
        => Result = new RationalPlaceholderExpression(newName);
}