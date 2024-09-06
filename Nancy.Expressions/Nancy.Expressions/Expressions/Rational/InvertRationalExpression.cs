﻿using Unipi.Nancy.Expressions.Visitors;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.Internals;

/// <summary>
/// Class representing an expression whose root operation is the inversion of a rational number
/// </summary>
public class InvertRationalExpression(RationalExpression expression, string expressionName = "", ExpressionSettings? settings = null)
    : RationalUnaryExpression<Rational>(expression, expressionName, settings)
{
    /// <summary>
    /// Creates a "rational inversion expression"
    /// </summary>
    public InvertRationalExpression(Rational number, string expressionName = "", ExpressionSettings? settings = null) : this(
        new RationalNumberExpression(number), expressionName, settings)
    {
    }
    
    /// <summary>
    /// Creates a "rational inversion expression"
    /// </summary>
    public override void Accept(IRationalExpressionVisitor visitor)
        => visitor.Visit(this);
}