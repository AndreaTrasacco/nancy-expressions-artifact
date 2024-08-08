namespace Unipi.Nancy.Expressions.Visitors;

public interface IVisitableCurve
{
    public void Accept(ICurveExpressionVisitor visitor);
}