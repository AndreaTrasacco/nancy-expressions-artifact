using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions.Visitors;

public interface IRationalExpressionVisitor : IExpressionVisitor
{
    public void Visit(HorizontalDeviationExpression expression);
    public void Visit(VerticalDeviationExpression expression);
    public void Visit(RationalAdditionExpression expression);
    public void Visit(RationalProductExpression expression);
    public void Visit(RationalDivisionExpression expression);
    public void Visit(RationalLeastCommonMultipleExpression expression);
    public void Visit(RationalGreatestCommonDivisorExpression expression);
    public void Visit(RationalNumberExpression expression);
    public void Visit(NegateRationalExpression expression);
    public void Visit(InvertRationalExpression expression);
    public void Visit(RationalPlaceholderExpression expression);
}