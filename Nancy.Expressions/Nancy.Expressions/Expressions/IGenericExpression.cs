using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.Expressions.ExpressionsUtility;
using Unipi.Nancy.Expressions.Visitors;

namespace Unipi.Nancy.Expressions.Internals;

/// <summary>
/// Interface which defines the rules each Nancy expression must follow.
/// </summary>
/// <typeparam name="T">T is the class (Curve or Rational) which
/// represents the type the expression evaluates to</typeparam>
public interface IGenericExpression<out T>
{
    /// <summary>
    /// The value of the expression
    /// </summary>
    public T Value { get; }
    
    /// <summary>
    /// The name of the expression
    /// </summary>
    public string Name { get; }

    public ExpressionSettings? Settings { get; }
    
    /// <summary>
    /// Computes the value the expression evaluates to
    /// </summary>
    public T Compute();
    
    /// <summary>
    /// Formats the expression in LaTeX.
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="showRationalsAsName"></param>
    public string ToLatexString(
        int depth = 20, 
        bool showRationalsAsName = false
    );

    /// <summary>
    /// Represents the expression in textual format
    /// </summary>
    public string ToString();

    /// <summary>
    /// Evaluates the computational complexity of the expression.
    /// </summary>
    /// <returns>The value representative of the complexity of the expression</returns>
    public double Estimate();

    /// <summary>
    /// Replaces every occurence of a sub-expression in the expression to which the method is applied.
    /// </summary>
    /// <param name="expressionPattern">The sub-expression to look for in the main expression for being replaced</param>
    /// <param name="newExpressionToReplace">The new sub-expression</param>
    /// <returns>New expression object with replaced sub-expressions</returns>
    public IGenericExpression<T> ReplaceByValue<T1>(IGenericExpression<T1> expressionPattern,
        IGenericExpression<T1> newExpressionToReplace);

    /// <summary>
    /// Replaces the sub-expression at a certain position in the expression to which the method is applied.
    /// </summary>
    /// <param name="expressionPosition">Position of the expression to be replaced</param>
    /// <param name="newExpression">The new sub-expression</param>
    /// <returns>New expression object with replaced sub-expression</returns>
    public IGenericExpression<T> ReplaceByPosition<T1>(ExpressionPosition expressionPosition,
        IGenericExpression<T1> newExpression);

    /// <summary>
    /// Replaces the sub-expression at a certain position in the expression to which the method is applied.
    /// </summary>
    /// <param name="positionPath">Position of the expression to be replaced. The position is expressed as a path from
    /// the root of the expression by using a list of strings "Operand" for unary operators, "LeftOperand"/"RightOperand"
    /// for binary operators, "Operand(index)" for n-ary operators</param>
    /// <param name="newExpression">The new sub-expression</param>
    /// <returns>New expression object with replaced sub-expression</returns>
    public IGenericExpression<T> ReplaceByPosition<T1>(IEnumerable<string> positionPath,
        IGenericExpression<T1> newExpression);

    /// <summary>
    /// Method used for implementing the Visitor design pattern: the visited object must "accept" the visitor object.
    /// </summary>
    /// <param name="visitor">The Visitor object</param>
    public void Accept(IExpressionVisitor visitor);

    /// <summary>
    /// Returns the representation of the expression using characters of the Unicode character set.
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="showRationalsAsName"></param>
    public string ToUnicodeString(
        int depth = 20, 
        bool showRationalsAsName = false
    );

    /// <summary>
    /// This method can be used as a starting point to build a symbolic path through the expression.
    /// </summary>
    /// <returns>Returns the position of the root of the expression</returns>
    public ExpressionPosition RootPosition();

    /// <summary>
    /// Changes the name of the expression.
    /// </summary>
    /// <param name="expressionName">The new name of the expression</param>
    /// <returns>The expression (new object) with the new name</returns>
    public IGenericExpression<T> WithName(string expressionName);

    public IGenericExpression<T> ApplyEquivalence(Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly);
    
    public IGenericExpression<T> ApplyEquivalenceByPosition(IEnumerable<string> positionPath, Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly);

    public IGenericExpression<T> ApplyEquivalenceByPosition(ExpressionPosition expressionPosition,
        Equivalence equivalence,
        CheckType checkType = CheckType.CheckLeftOnly);
}
