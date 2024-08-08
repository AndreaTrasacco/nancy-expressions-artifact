using System.Text;
using System.Text.RegularExpressions;
using Nancy.Expressions.Expressions;
using Unipi.Nancy.Expressions.Internals;

namespace Unipi.Nancy.Expressions.Visitors;

public partial class LatexFormatterVisitor(int depth = 20, bool showRationalsAsName = true)
    : ICurveExpressionVisitor, IRationalExpressionVisitor
{
    public StringBuilder Result { get; } = new();
    private static readonly List<string> GreekLetters =
    [
        "alpha", "beta", "gamma", "delta", "epsilon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu",
        "xi", "omicron", "pi", "rho", "sigma", "tau", "upsilon", "phi", "chi", "psi", "omega"
    ];
    
    // todo: explore having generic entry points that take a PreferredOperatorNotation enum or something similar
    
    private void VisitBinaryInfix<T1, T2, TResult>(
        IGenericBinaryExpression<T1, T2, TResult> expression,
        string latexOperation
    )
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("\\left(");
        expression.LeftExpression.Accept(this);
        Result.Append(latexOperation);
        expression.RightExpression.Accept(this);
        Result.Append("\\right)");
        depth++;
    }

    private void VisitBinaryPrefix<T1, T2, TResult>(
        IGenericBinaryExpression<T1, T2, TResult> expression,
        string latexOperation
    )
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append(latexOperation);
        Result.Append("\\left(");
        expression.LeftExpression.Accept(this);
        Result.Append(", ");
        expression.RightExpression.Accept(this);
        Result.Append("\\right)");
        depth++;
    }
    
    private void VisitBinaryCommand<T1, T2, TResult>(
        IGenericBinaryExpression<T1, T2, TResult> expression,
        string latexCommand
    )
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append(latexCommand);
        Result.Append('{');
        expression.LeftExpression.Accept(this);
        Result.Append("}{");
        expression.RightExpression.Accept(this);
        Result.Append('}');
        depth++;
    }

    private void VisitNAryInfix<T, TResult>(
        IGenericNAryExpression<T, TResult> expression, 
        string latexOperation
    )
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("\\left(");
        var c = expression.Expressions.Count;
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            c--;
            if (c > 0)
                Result.Append(latexOperation);
        }

        Result.Append("\\right)");
        depth++;
    }

    private void VisitNAryPrefix<T, TResult>(
        IGenericNAryExpression<T, TResult> expression, 
        string latexOperation
    )
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append(latexOperation);
        Result.Append("\\left(");
        var c = expression.Expressions.Count;
        foreach (var e in expression.Expressions)
        {
            e.Accept(this);
            c--;
            if (c > 0)
                Result.Append(", ");
        }

        Result.Append("\\right)");
        depth++;
    }

    private void FormatByName(string name)
    {
        var match = MyRegex().Match(name);
        string nameLetters;
        string? nameNumber = null;
        if (match.Success)
        {
            nameLetters = match.Groups[1].Value;
            nameNumber = match.Groups[2].Value;
        }
        else
            nameLetters = name;

        var nameLettersLower = nameLetters.ToLower();
        if (GreekLetters.Any(s => s.Equals(nameLettersLower)))
            Result.Append("\\" + nameLettersLower);
        else
            Result.Append(nameLetters);

        Result.Append(nameNumber != null ? "_{{" + nameNumber + "}" : "");

        Result.Append(nameNumber != null ? "}" : "");
    }

    public void Visit(ConcreteCurveExpression expression)
    {
        FormatByName(expression.Name);
    }

    public void Visit(RationalAdditionExpression expression)
        => VisitNAryInfix(expression, " + ");

    public void Visit(RationalProductExpression expression)
        => VisitNAryInfix(expression, " \\cdot ");

    public void Visit(RationalDivisionExpression expression)
        => VisitBinaryCommand(expression, "\\frac");

    public void Visit(RationalLeastCommonMultipleExpression expression)
        => VisitNAryPrefix(expression, "\\operatorname{lcm}");

    public void Visit(RationalGreatestCommonDivisorExpression expression)
        => VisitNAryPrefix(expression, "\\operatorname{gcd}");

    public void Visit(RationalNumberExpression numberExpression)
    {
        if (!numberExpression.Name.Equals("") && (showRationalsAsName || depth <= 0))
        {
            FormatByName(numberExpression.Name);
            return;
        }

        Result.Append($"\\frac{numberExpression.Value.Numerator}{numberExpression.Value.Denominator}");
    }

    public void Visit(NegateExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append('-');
        expression.Expression.Accept(this);
        depth++;
    }

    public void Visit(ToNonNegativeExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        var squareParenthesis = expression.Expression is not (ConcreteCurveExpression
            or ToUpperNonDecreasingExpression
            or ToLowerNonDecreasingExpression);
        if (squareParenthesis) Result.Append('[');
        expression.Expression.Accept(this);
        if (squareParenthesis) Result.Append(']');
        string resultToString = Result.ToString();
        // Usually ToNonNegative is used together with ToUpperNonDecreasing or ToLowerNonDecreasing
        // The following instructions are used to obtain the proper Latex formatting
        if (resultToString.EndsWith("{_\\uparrow}") || resultToString.EndsWith("{_\\downarrow}"))
        {
            Result.Remove(Result.Length - 1, 1);
            Result.Append("^{+}}");
        }
        else
            Result.Append("{^{+}}");

        depth++;
    }

    public void Visit(SubAdditiveClosureExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append(" \\overline {");
        expression.Expression.Accept(this);
        Result.Append("} ");
        depth++;
    }

    public void Visit(SuperAdditiveClosureExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append(@" \overline{\overline{");
        expression.Expression.Accept(this);
        Result.Append("}} ");
        depth++;
    }

    public void Visit(ToUpperNonDecreasingExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        var squareParenthesis = expression.Expression is not (ConcreteCurveExpression or ToNonNegativeExpression);
        if (squareParenthesis) Result.Append('[');
        expression.Expression.Accept(this);
        if (squareParenthesis) Result.Append(']');
        // Usually ToUpperNonDecreasing is used together with ToNonNegative 
        // The following instructions are used to obtain the proper Latex formatting
        if (Result.ToString().EndsWith("{^{+}}"))
        {
            Result.Remove(Result.Length - 1, 1);
            Result.Append("_\\uparrow}");
        }
        else
            Result.Append("{_\\uparrow}");

        depth++;
    }

    public void Visit(ToLowerNonDecreasingExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        var squareParenthesis = expression.Expression is not (ConcreteCurveExpression or ToNonNegativeExpression);
        if (squareParenthesis) Result.Append('[');
        expression.Expression.Accept(this);
        if (squareParenthesis) Result.Append(']');
        // Usually ToLowerNonDecreasing is used together with ToNonNegative 
        // The following instructions are used to obtain the proper Latex formatting
        if (Result.ToString().EndsWith("{^{+}}"))
        {
            Result.Remove(Result.Length - 1, 1);
            Result.Append("_\\downarrow}");
        }
        else
            Result.Append("{_\\downarrow}");

        depth++;
    }

    public void Visit(ToLeftContinuousExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("toLeftContinuous ");
        expression.Expression.Accept(this);
        depth++;
    }

    public void Visit(ToRightContinuousExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("toRightContinuous ");
        expression.Expression.Accept(this);
        depth++;
    }

    public void Visit(WithZeroOriginExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("\\left(");
        expression.Expression.Accept(this);
        Result.Append("°)");
        depth++;
    }

    public void Visit(LowerPseudoInverseExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        expression.Expression.Accept(this);
        Result.Append("{_\\downarrow^{-1}}");
        depth++;
    }

    public void Visit(UpperPseudoInverseExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        expression.Expression.Accept(this);
        Result.Append("{_\\uparrow^{-1}}");
        depth++;
    }

    public void Visit(AdditionExpression expression)
        => VisitNAryInfix(expression, "+");

    public void Visit(SubtractionExpression expression)
        => VisitBinaryInfix(expression, "-");

    public void Visit(MinimumExpression expression)
        => VisitNAryInfix(expression, " \\wedge ");

    public void Visit(MaximumExpression expression)
        => VisitNAryInfix(expression, " \\vee ");

    public void Visit(ConvolutionExpression expression)
        => VisitNAryInfix(expression, " \\otimes ");

    public void Visit(DeconvolutionExpression expression)
        => VisitBinaryInfix(expression, " \\oslash ");

    public void Visit(MaxPlusConvolutionExpression expression)
        => VisitNAryInfix(expression, @" \overline{\otimes} ");

    public void Visit(MaxPlusDeconvolutionExpression expression)
        => VisitBinaryInfix(expression, @" \overline{\oslash} ");

    public void Visit(CompositionExpression expression)
        => VisitBinaryInfix(expression, " \\circ ");

    public void Visit(DelayByExpression expression)
        => VisitBinaryPrefix(expression, " delayBy");

    public void Visit(AnticipateByExpression expression)
        => VisitBinaryPrefix(expression, " anticipateBy");

    public void Visit(NegateRationalExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append('-');
        expression.Expression.Accept(this);
        depth++;
    }

    public void Visit(InvertRationalExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        var parenthesis = expression.Expression is RationalNumberExpression;
        if (parenthesis)
            Result.Append("\\left(");
        expression.Expression.Accept(this);
        if (parenthesis)
            Result.Append("\\right)");
        Result.Append("^{-1}");
        depth++;
    }

    public void Visit(HorizontalDeviationExpression expression)
        => VisitBinaryPrefix(expression, "hdev");

    public void Visit(VerticalDeviationExpression expression)
        => VisitBinaryPrefix(expression, "vdev");

    public void Visit(CurvePlaceholderExpression expression)
        => Result.Append(expression.Name);

    public void Visit(RationalPlaceholderExpression expression)
        => Result.Append(expression.Name);

    public void Visit(ScaleExpression expression)
    {
        if (depth <= 0 && !expression.Name.Equals(""))
        {
            FormatByName(expression.Name);
            return;
        }

        depth--;
        Result.Append("\\left(");
        expression.RightExpression.Accept(this);
        Result.Append(" \\cdot ");
        expression.LeftExpression.Accept(this);
        Result.Append("\\right)");
        depth++;
    }

    [GeneratedRegex("^(.*?)(\\d+)$")]
    private static partial Regex MyRegex();
}