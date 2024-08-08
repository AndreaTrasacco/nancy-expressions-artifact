namespace Unipi.Nancy.Expressions.ExpressionsUtility;

/// <summary>
/// Class which models the position of a sub-expression inside a DNC expression. The position is obtained by
/// specifying, using the different methods, the path from the root of the expression tree to the node representing
/// the sub-expression.
/// </summary>
public class ExpressionPosition()
{
    private readonly IEnumerable<string> _positionPath = [];

    public ExpressionPosition(IEnumerable<string> positionPath) : this()
    {
        var enumerable = positionPath.ToList();
        if (ValidateExpressionPosition(enumerable))
            _positionPath = enumerable;
        else
            throw new ArgumentException("Invalid position!", nameof(positionPath));
    }

    public ExpressionPosition Operand() // For unary expressions
        => new(_positionPath.Append("Operand"));

    public ExpressionPosition Operand(int index) // For n-ary expressions
        => new(_positionPath.Append(index.ToString()));

    public ExpressionPosition LeftOperand() // For binary expressions
        => new(_positionPath.Append("LeftOperand"));

    public ExpressionPosition RightOperand() // For binary expressions
        => new(_positionPath.Append("RightOperand"));

    private static bool IsNumber(string input)
    {
        return int.TryParse(input, out _);
    }

    private static bool IsValidPosition(string input)
    {
        return input == "Operand" || input == "LeftOperand" || input == "RightOperand" || IsNumber(input);
    }

    public static bool ValidateExpressionPosition(IEnumerable<string> positionPath)
    {
        return positionPath.All(IsValidPosition);
    }

    public IEnumerable<string> GetPositionPath() => _positionPath;
}