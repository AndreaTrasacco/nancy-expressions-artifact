﻿using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.MinPlusAlgebra;

namespace Unipi.Nancy.Expressions.Internals;

public class ToLeftContinuousExpression(CurveExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : CurveUnaryExpression<Curve>(expression, expressionName, settings)
{
    public ToLeftContinuousExpression(Curve curve, string name, string expressionName = "", ExpressionSettings? settings = null) : this(
        new ConcreteCurveExpression(curve, name), expressionName, settings)
    {
    }

    public override void Accept(ICurveExpressionVisitor visitor)
        => visitor.Visit(this);
}