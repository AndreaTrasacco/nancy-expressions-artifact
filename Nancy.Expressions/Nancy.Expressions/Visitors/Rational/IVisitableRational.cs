namespace Unipi.Nancy.Expressions.Visitors;

public interface IVisitableRational
{
    public void Accept(IRationalExpressionVisitor visitor);
}