﻿using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class ToNonNegativeExpression(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : CurveUnaryExpression<Curve>(expression, expressionName, settings)
{
    public ToNonNegativeExpression(Curve curve, string name, string expressionName = "", ExpressionSettings? settings = null) : this(
        new ConcreteCurveExpression(curve, name), expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}