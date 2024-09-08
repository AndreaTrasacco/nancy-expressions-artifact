# *Nancy* VS *Nancy.Expressions*

This example shows a comparison between *Nancy* and *Nancy.Expressions* in terms of syntax and behavior.

## Syntax
The syntax of *Nancy.Expressions* is close to the one already used in *Nancy*, allowing for easy conversion of existing software. 

### *Nancy*
```
var f = new Curve(
    <constructor arguments for f>
);
var g = new Curve(
    <constructor arguments for g>
);
var f_sac = f.SubAdditiveClosure();
var conv = Curve.Convolution(f_sac, g);
var result = Curve.Deconvolution(conv, f_sac);
```

### *Nancy.Expressions*
```
var f = Expressions.FromCurve(
    curve: new Curve(
        <constructor arguments for f>
    );
var g = Expressions.FromCurve(
    curve: new Curve(
        <constructor arguments for g>
    );
var f_sac = f.SubAdditiveClosure();
var conv = Expressions.Convolution(f_sac, g);
var deconv = Expressions.Deconvolution(conv, f_sac);
var result = deconv.Compute();
```

## Behavior

The behavior of the two libraries in evaluating a DNC expression differ in the actual moment in which computations are performed.

### *Nancy*

Each method of the *Nancy* library, such as `f.SubAdditiveClosure()`, `Curve.Convolution(f_sac, g)` or `Curve.Deconvolution(conv, f_sac)`, immediately computes the resulting curve.

### *Nancy.Expressions*

The methods `f.SubAdditiveClosure()`, `Expressions.Convolution(f_sac, g)` or `Expressions.Deconvolution(conv, f_sac)`, don't make any computation to allow the construction and successive analysis of the expression and possible simplification. The actual computation of the expression is performed calling the `Compute()` method and exploiting the API of the *Nancy* library.

