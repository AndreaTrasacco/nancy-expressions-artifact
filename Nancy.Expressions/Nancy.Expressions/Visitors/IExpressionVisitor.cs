using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions.Visitors;

/// <summary>
/// Visitor interface of the Visitor design pattern. It allows to keep separated an algorithm ("visit") from the
/// complex data structure it is applied to, so that it is possible to implement new algorithms without modifying
/// the complex data structure itself.
/// </summary>
public interface IExpressionVisitor
{
    void Visit<T>(IGenericExpression<T> expression)
        => throw new InvalidOperationException("Missing Visit method for type " + expression.GetType());
}