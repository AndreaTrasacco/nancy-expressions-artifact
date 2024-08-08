using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.Numerics;

namespace Unipi.Nancy.Expressions.ExpressionsUtility;

public static class ExtensionMethods
{
    public static bool IsZeroAtZero(this Curve curve)
    {
        return curve.ValueAt(Rational.Zero) == Rational.Zero;
    }

    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        var enumerable = list.ToList();
        return GetCombinations(enumerable, length - 1)
            .SelectMany(t => enumerable.Where(o => !t.Contains(o)),
                (t1, t2) => t1.Concat(new[] { t2 }));
    }
}