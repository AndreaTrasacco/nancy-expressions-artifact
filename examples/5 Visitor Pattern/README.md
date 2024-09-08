# Visitor Pattern

This notebook shows an example about visitor classes (following the _Visitor_ design pattern) to extend the functionalities of _Nancy.Expressions_.
In the example, we don't define a visitor class from scratch but we extend the visitor which formats the expressions in Latex syntax.
After the extension of the visitor, we show how to use it.

The following code cell defines the new (trivial) visitor class in which we override only the `Visit` method of the convolution expression.
The extended behavior is simply the following: if all the operands of the convolution have the same $name$, then the convolution is shown as $\bigotimes_{i = 1 \dots n}{name_i}$ using an index $i$.

```
public class LatexFormatterExtended : LatexFormatterVisitor
{
    public override void Visit(ConvolutionExpression expression)
    {
        var name = "";
        foreach (var expr in expression.Expressions)
        {
            if (name.Equals("")) name = expr.Name;
            if (expr.Name.Equals(name)) continue;
            name = "_";
            break;
        }
        if(name.Equals("_")) base.Visit(expression);
        else {        
            Result.Append("\\left(\\bigotimes_{i = 1 \\dots " + expression.Expressions.Count + "}{");
            FormatName(name);
            Result.Append("_i");
            Result.Append("}\\right)");
        }
    }
}
```

In the following code cell, it is shown how to use the new visitor:

```
var latexFormatter = new LatexFormatterExtended();
<initialization of curves>
var conv = Expressions.Convolution([beta1, beta2, beta3, beta4], ["beta", "beta", "beta", "beta"]);
var hdev = Expressions.HorizontalDeviation(alpha, conv);

hdev.Accept(latexFormatter);
LatexRenderer.ToHtml(latexFormatter.Result.ToString())
             .DisplayAs(HtmlFormatter.MimeType);
```

Output: $hdev\left(\alpha, \left(\bigotimes_{i = 1 \dots 4}{\beta_i}\right)\right)$.