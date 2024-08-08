using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions;

public static class RationalExpressionExtensions
{
    public static RationalAdditionExpression Sum(this IEnumerable<RationalExpression> rationalExpressions)
        => new RationalAdditionExpression(rationalExpressions.ToList());

    public static RationalAdditionExpression Sum(this IReadOnlyCollection<RationalExpression> rationalExpressions)
        => new RationalAdditionExpression(rationalExpressions);
}