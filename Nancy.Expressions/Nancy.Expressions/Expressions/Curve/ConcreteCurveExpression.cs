using System.Runtime.CompilerServices;
using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class ConcreteCurveExpression : CurveExpression
{
    public ConcreteCurveExpression() : this(Curve.Zero(), "defaultCurve")
    {
    }

    public ConcreteCurveExpression(Curve curve,
        [CallerArgumentExpression("curve")] string name = "",
        ExpressionSettings? settings = null) : base(name, settings)
    {
        _value = curve;
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}
