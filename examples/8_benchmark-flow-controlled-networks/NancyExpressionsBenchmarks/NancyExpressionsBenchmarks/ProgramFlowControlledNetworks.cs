using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using dnc_expression_evaluation_benchmark;
using Unipi.Nancy.Expressions;
using Unipi.Nancy.Expressions.Equivalences;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.NetworkCalculus;

{
    // SanityCheck();
    BenchmarkSwitcher
        .FromAssembly(typeof(Program).Assembly)
        .Run(args,
            DefaultConfig.Instance
                .WithSummaryStyle(SummaryStyle.Default.WithMaxParameterColumnWidth(10000))
                .WithCultureInfo(CultureInfo.InvariantCulture));
}
return;

void SanityCheck()
{
    Console.WriteLine("Running sanity check");

    {
        Console.WriteLine("FlowControlledNetworksBenchmark");
        var benchmarkRunner = new FlowControlledNetworksParametricBenchmark();
        foreach (var n in (int[]) [4, 8, 10, 12, 14])
        {
            foreach (var rng in (int[]) [2345])
            {
                benchmarkRunner.n = n;
                benchmarkRunner.RNG_SEED = rng;
                benchmarkRunner.GlobalSetup();
                var curveResultNancy = benchmarkRunner.Nancy();
                var curveResultNancyExpressions = benchmarkRunner.NancyExpressions();
                var curveResultNancyExpressionsEquivalence = benchmarkRunner.NancyExpressionsEquivalence();
                if (!curveResultNancy.Equivalent(curveResultNancyExpressions) ||
                    !curveResultNancy.Equivalent(curveResultNancyExpressionsEquivalence))
                {
                    Console.WriteLine("SANITY FAIL!");
                }
                else
                {
                    Console.Write(".");
                }
            }
        }
    }

    Console.WriteLine();
    Console.WriteLine();
}

namespace dnc_expression_evaluation_benchmark
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class FlowControlledNetworksParametricBenchmark
    {
        [Params(4, 8, 10, 12, 14)] public int n;
        [Params(2345)] public int RNG_SEED;

        public Curve[] beta;
        public Curve[] W;

        [GlobalSetup]
        public void GlobalSetup()
        {
            beta = new Curve[n];
            W = new Curve[n]; // W[0] not needed

            var rnd = new Random(RNG_SEED);
            for (var i = 0; i < n; i++)
            {
                beta[i] = new RateLatencyServiceCurve(rnd.Next(2, 20), rnd.Next(2, 20));
                W[i] = new ConstantCurve(rnd.Next(2, 100));
            }
        }

        [Benchmark(Baseline = true)]
        public Curve Nancy()
        {
            var temp = new Curve[n - 1];
            for (var j = 0; j < n - 1; j++)
            {
                temp[j] = Curve.Convolution(beta[j], beta[j + 1]).Addition(W[j + 1]).SubAdditiveClosure();
            }

            List<Curve> array = [];
            for (var i = 0; i < n; i++)
            {
                array.Add(beta[i]);
                for (var j = i; j < n - 1; j++)
                {
                    array.Add(temp[j]);
                }
            }

            return Curve.Convolution(array, ComputationSettings.Default() with { UseParallelism = false });
        }

        [Benchmark]
        public Curve NancyExpressions()
        {
            var settings = new ExpressionSettings
                { ComputationSettings = ComputationSettings.Default() with { UseParallelism = false } };
            var temp = new CurveExpression[n - 1];
            for (int j = 0; j < n - 1; j++)
            {
                temp[j] = Expressions.Convolution(beta[j], beta[j + 1], "beta" + (j + 1), "beta" + (j + 2))
                    .Addition(W[j + 1], "W" + (j + 2)).SubAdditiveClosure();
            }

            var beta_eq = new CurveExpression[n];
            for (var i = 0; i < n; i++)
            {
                beta_eq[i] = Expressions.FromCurve(beta[i], "beta" + (i + 1));
                for (var j = i; j < n - 1; j++)
                {
                    beta_eq[i] = beta_eq[i].Convolution(temp[j], settings: settings);
                }
            }

            var resultExpression = beta_eq[0];
            for (var i = 1; i < n; i++)
            {
                resultExpression = Expressions.Convolution(resultExpression, beta_eq[i], settings: settings);
            }

            return resultExpression.Compute();
        }

        [Benchmark]
        public Curve NancyExpressionsEquivalence()
        {
            var settings = new ExpressionSettings
                { ComputationSettings = ComputationSettings.Default() with { UseParallelism = false } };
            var temp = new CurveExpression[n - 1];
            for (var j = 0; j < n - 1; j++)
            {
                temp[j] = Expressions.Convolution(beta[j], beta[j + 1], "beta" + (j + 1), "beta" + (j + 2))
                    .Addition(W[j + 1], "W" + (j + 2)).SubAdditiveClosure();
            }

            var beta_eq = new CurveExpression[n];
            for (var i = 0; i < n - 1; i++)
            {
                beta_eq[i] = Expressions.FromCurve(beta[i], "beta" + (i + 1));
                for (var j = i; j < n - 1; j++)
                {
                    beta_eq[i] = beta_eq[i].Convolution(temp[j]);
                }
            }

            beta_eq[n - 1] = Expressions.FromCurve(beta[n - 1], "beta" + (n));
            var resultExpression = beta_eq[0];
            for (var i = 1; i < n; i++)
            {
                resultExpression = Expressions.Convolution(resultExpression, beta_eq[i], settings: settings);
            }


            var selfConv = new SelfConvolutionSubAdditive();
            var equivalentExpression = resultExpression;
            do
            {
                resultExpression = equivalentExpression;
                equivalentExpression = resultExpression.ApplyEquivalence(selfConv);
            } while (equivalentExpression != resultExpression);

            return equivalentExpression.Compute();
        }
    }
}