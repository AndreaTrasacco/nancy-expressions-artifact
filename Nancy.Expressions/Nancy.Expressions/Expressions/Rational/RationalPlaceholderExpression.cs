using Unipi.Nancy.Expressions.Visitors;

namespace Unipi.Nancy.Expressions.Internals;

public class RationalPlaceholderExpression(
    string rationalName,
    ExpressionSettings? settings = null) : RationalExpression(rationalName, settings)
{
    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}