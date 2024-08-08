using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions;

public static class CurveExpressionExtensions
{
    public static AdditionExpression Sum(this IEnumerable<CurveExpression> curveExpressions)
        => new AdditionExpression(curveExpressions.ToList());

    public static AdditionExpression Sum(this IReadOnlyCollection<CurveExpression> curveExpressions)
        => new AdditionExpression(curveExpressions);
}