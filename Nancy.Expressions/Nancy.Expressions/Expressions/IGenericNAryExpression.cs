using Unipi.Nancy.Expressions.Internals;

namespace Nancy.Expressions.Expressions;

public interface IGenericNAryExpression<out T1, out TResult> : IGenericExpression<TResult>
{
    public IReadOnlyCollection<IGenericExpression<T1>> Expressions { get; }
}