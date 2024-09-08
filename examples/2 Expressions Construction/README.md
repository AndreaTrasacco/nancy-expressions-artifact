# Expressions Construction

This example shows how to use the interface provided by *Nancy.Expressions* to construct the expression $[(\beta_{{1}}-\alpha_{{1}})]{_\uparrow^{+}} \otimes [(\beta_{{4}}-((\alpha_{{3}}+\alpha_{{4}}) \oslash \beta_{{3}}))]{_\uparrow^{+}}$.

First of all, we need to declare and initialize the curves in the expression.

```
var beta1 = new RateLatencyServiceCurve(1,1);
var beta2 = new RateLatencyServiceCurve(1,1);
var beta3 = new RateLatencyServiceCurve(1,1);
var beta4 = new RateLatencyServiceCurve(1,1);
var alpha1 = new SigmaRhoArrivalCurve(1,1);
var alpha3 = new SigmaRhoArrivalCurve(1,1);
var alpha4 = new SigmaRhoArrivalCurve(1,1);
```

By adopting the API offered by the class `Expressions`of the *Nancy.Expressions* library, the expression can be easily constructed.

```
var subLeft = Expressions.Subtraction(beta1, alpha1)
    .ToUpperNonDecreasing()
    .ToNonNegative();
var deconv = Expressions.Addition(alpha3, alpha4).Deconvolution(beta3);
var subRight = Expressions.Subtraction(beta4, deconv)
    .ToUpperNonDecreasing()
    .ToNonNegative();
var e2eServiceCurve2Exp = Expressions.Convolution(subLeft, subRight);
```

Inside a notebook, it is possible to print the expression in Latex format using the following code:

```
display(e2eServiceCurve2Exp);
```

The output is: $[(\beta_{{1}}-\alpha_{{1}})]{_\uparrow^{+}} \otimes [(\beta_{{4}}-((\alpha_{{3}}+\alpha_{{4}}) \oslash \beta_{{3}}))]{_\uparrow^{+}}$.