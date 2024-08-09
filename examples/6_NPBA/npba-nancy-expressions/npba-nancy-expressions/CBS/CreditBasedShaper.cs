using System.Numerics;
using Unipi.Nancy.Expressions;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.NetworkCalculus;
using Unipi.Nancy.Numerics;

namespace CBS;

public class CreditBasedShaper
{
    public CreditBasedShaper(BigInteger maxFrameSizeA, BigInteger maxFrameSizeB, BigInteger maxFrameSizeNsr,
        bool isStrict)
    {
        MaxFrameSizeA = maxFrameSizeA * 8;
        MaxFrameSizeB = maxFrameSizeB * 8;
        MaxFrameSizeNsr = maxFrameSizeNsr * 8;
        MaxFrameSizeN = BigInteger.Max(MaxFrameSizeB, MaxFrameSizeNsr);
        Flows = Array.Empty<Flow>();
        IsStrict = isStrict;
    }

    public BigInteger MaxFrameSizeA { get; }
    public BigInteger MaxFrameSizeB { get; }
    public BigInteger MaxFrameSizeNsr { get; }
    public BigInteger MaxFrameSizeN { get; }
    public IList<Flow> Flows { get; private set; }
    public bool IsStrict { get; }

    public void SetFlows(IList<Flow> flows) => Flows = flows;

    public static RationalExpression ComputeEndToEndDelay(Flow flow)
    {
        var delay = flow.Path
            .Select(link => link.ComputeDelay(flow.AvbClass).WithName("delay" + link.Name))
            .Sum().WithName("e2eDelayAllFlows");
        
        return delay;
    }

    public static RationalExpression ComputeLinkDelayA(Link link) => ComputeLinkDelay(link, AvbClass.A);
    public static RationalExpression ComputeLinkDelayB(Link link) => ComputeLinkDelay(link, AvbClass.B);
    public static RationalExpression ComputeLinkDelay(Link link, AvbClass avbClass) => link.ComputeDelay(avbClass);


    public static RationalExpression ComputeLinkBacklogA(Link link) => ComputeLinkBacklog(link, AvbClass.A);
    public static RationalExpression ComputeLinkBacklogB(Link link) => ComputeLinkBacklog(link, AvbClass.B);
    public static RationalExpression ComputeLinkBacklog(Link link, AvbClass avbClass) => link.ComputeBacklog(avbClass);


    public static CurveExpression MakePeriodicArrivalCurve(Flow flow)
        //Theorem 1
    {
        var link = flow.Path[0];
        var linkSpeed = link.LinkSpeed;
        var m = flow.MaxIntervalFrame * flow.MaxFrameSize * 8;
        var r = m / flow.ClassMeasurementInterval;
        var b = m * (1 - r / linkSpeed);
        var ac = new SigmaRhoArrivalCurve(b, r);
        var linkSpeedCurve = new SigmaRhoArrivalCurve(0, linkSpeed);
        return Expressions.Minimum(linkSpeedCurve, ac, $"linkspeed{link.Name}", $"th_1br{flow.Name}");
    }

    public static CurveExpression MakePeriodicWorstCaseArrivalCurve(Flow flow)
        //Theorem 1
    {
        var linkSpeed = flow.Path[0].LinkSpeed;
        var m = flow.MaxIntervalFrame * flow.MaxFrameSize * 8;
        var linkSpeedCurve = new SigmaRhoArrivalCurve(0, linkSpeed);
        var stair = new StairCurve(m, flow.ClassMeasurementInterval);
        return Expressions.Convolution(stair, linkSpeedCurve, $"th_1stair{flow.Name}", $"linkspeed{flow.Path[0].Name}");
    }

    public static CurveExpression MakeNonPeriodicArrivalCurve(Flow flow)
        //Theorem 2
    {
        var link = flow.Path[0];
        var linkSpeed = link.LinkSpeed;
        var m = flow.MaxIntervalFrame * flow.MaxFrameSize * 8;
        var r = m / flow.ClassMeasurementInterval;
        var b = m * (1 - r / linkSpeed);
        var ac = new SigmaRhoArrivalCurve(2 * b, r);
        var linkSpeedCurve = new SigmaRhoArrivalCurve(0, linkSpeed);
        return Expressions.Minimum(linkSpeedCurve, ac, $"linkspeed{link.Name}", $"th_2br{flow.Name}");
    }

    public static CurveExpression MakeNonPeriodicWorstCaseArrivalCurve(Flow flow)
        //Theorem 2
    {
        var link = flow.Path[0];
        var linkSpeed = link.LinkSpeed;
        var m = flow.MaxIntervalFrame * flow.MaxFrameSize * 8;
        var basicStair = new StairCurve(1, flow.ClassMeasurementInterval);
        var addedStair = Expressions.Addition(basicStair, new ConstantCurve(1), $"th_2basicStair{flow.Name}").WithZeroOrigin();
        var scaledStair = addedStair.Scale(m);
        var linkSpeedCurve = new SigmaRhoArrivalCurve(0, linkSpeed);
        return Expressions.Convolution(scaledStair, linkSpeedCurve, $"linkspeed_{link.Name}");
    }


    public static CurveExpression MakeStrictMinimalServiceCurveA(Link link) =>
        MakeStrictMinimalServiceCurveA(link, link.Cbs.MaxFrameSizeA, link.Cbs.MaxFrameSizeN);

    public static CurveExpression MakeStrictMinimalServiceCurveA(Link link, BigInteger maxFrameSizeA,
            BigInteger maxFrameSizeN)
        //Theorem 3
    {
        var rate = new Rational((link.IdleSlopeA * link.LinkSpeed), link.IdleSlopeA - link.SendSlopeA);
        var latency = new Rational(maxFrameSizeN, link.LinkSpeed) -
                      new Rational(maxFrameSizeA * link.SendSlopeA, link.IdleSlopeA * link.LinkSpeed);
        return Expressions.FromCurve(new RateLatencyServiceCurve(rate, latency), $"strictminscA{link.Name}");
    }


    public static CurveExpression MakeMinimalServiceCurveA(Link link) =>
        MakeMinimalServiceCurveA(link, link.Cbs.MaxFrameSizeN);

    public static CurveExpression MakeMinimalServiceCurveA(Link link, BigInteger maxFrameSizeN)
        //Theorem 4
    {
        var rate = new Rational((link.IdleSlopeA * link.LinkSpeed), link.IdleSlopeA - link.SendSlopeA);
        var latency = new Rational(maxFrameSizeN, link.LinkSpeed);
        return Expressions.FromCurve(new RateLatencyServiceCurve(rate, latency), $"minscA{link.Name}");
    }


    public static CurveExpression MakeShaperCurveA(Link link) =>
        MakeShaperCurveA(link, link.Cbs.MaxFrameSizeA, link.Cbs.MaxFrameSizeN);

    public static CurveExpression MakeShaperCurveA(Link link, BigInteger maxFrameSizeA, BigInteger maxFrameSizeN)
        //Theorem 5
    {
        var rate = new Rational(link.IdleSlopeA * link.LinkSpeed, link.IdleSlopeA - link.SendSlopeA);
        var burst = rate * (new Rational(maxFrameSizeN, link.LinkSpeed)
                            - new Rational(maxFrameSizeA * link.SendSlopeA, link.IdleSlopeA * link.LinkSpeed));
        return Expressions.FromCurve(new SigmaRhoArrivalCurve(burst, rate), $"shaperA{link.Name}");
    }


    public static CurveExpression MakeMaximalServiceCurveA(Link link) =>
        MakeMaximalServiceCurveA(link, link.Cbs.MaxFrameSizeA);

    public static CurveExpression MakeMaximalServiceCurveA(Link link, BigInteger maxFrameSizeA)
        //Theorem 6
    {
        var rate = new Rational(link.IdleSlopeA * link.LinkSpeed, link.IdleSlopeA - link.SendSlopeA);
        var burst = rate * -new Rational(maxFrameSizeA * link.SendSlopeA, link.IdleSlopeA * link.LinkSpeed);
        return Expressions.FromCurve(new SigmaRhoArrivalCurve(burst, rate), $"maxscA{link.Name}");
    }


    public static CurveExpression MakeStrictMinimalServiceCurveB(Link link) =>
        MakeStrictMinimalServiceCurveB(link, link.Cbs.MaxFrameSizeA, link.Cbs.MaxFrameSizeB, link.Cbs.MaxFrameSizeNsr);

    public static CurveExpression MakeStrictMinimalServiceCurveB(Link link, BigInteger maxFrameSizeA,
            BigInteger maxFrameSizeB, BigInteger maxFrameSizeNsr)
        //Theorem 7
    {
        var maxFrameSizeN = BigInteger.Max(maxFrameSizeB, maxFrameSizeNsr);
        var rate = new Rational((link.IdleSlopeB * link.LinkSpeed), link.IdleSlopeB - link.SendSlopeB);
        var latency = new Rational(maxFrameSizeNsr + maxFrameSizeA, link.LinkSpeed)
                      - new Rational(maxFrameSizeN * link.IdleSlopeA, link.LinkSpeed * link.SendSlopeA)
                      - new Rational(maxFrameSizeB * link.SendSlopeB, link.LinkSpeed * link.IdleSlopeB);
        return Expressions.FromCurve(new RateLatencyServiceCurve(rate, latency), $"scritminscB{link.Name}");
    }


    public static CurveExpression MakeMinimalServiceCurveB(Link link) =>
        MakeMinimalServiceCurveB(link, link.Cbs.MaxFrameSizeA, link.Cbs.MaxFrameSizeNsr, link.Cbs.MaxFrameSizeN);

    public static CurveExpression MakeMinimalServiceCurveB(Link link, BigInteger maxFrameSizeA,
            BigInteger maxFrameSizeNsr, BigInteger maxFrameSizeN)
        //Theorem 8
    {
        var rate = new Rational((link.IdleSlopeB * link.LinkSpeed), link.IdleSlopeB - link.SendSlopeB);
        var latency = new Rational(maxFrameSizeNsr + maxFrameSizeA, link.LinkSpeed)
                      - new Rational(maxFrameSizeN * link.IdleSlopeA, link.LinkSpeed * link.SendSlopeA);
        return Expressions.FromCurve(new RateLatencyServiceCurve(rate, latency), $"minscB{link.Name}");
    }


    public static CurveExpression MakeShaperCurveB(Link link) =>
        MakeShaperCurveB(link, link.Cbs.MaxFrameSizeA, link.Cbs.MaxFrameSizeB, link.Cbs.MaxFrameSizeNsr);

    public static CurveExpression MakeShaperCurveB(Link link, BigInteger maxFrameSizeA, BigInteger maxFrameSizeB,
            BigInteger maxFrameSizeNsr)
        //Theorem 9
    {
        var maxFrameSizeN = BigInteger.Max(maxFrameSizeB, maxFrameSizeNsr);
        var rate = new Rational(link.IdleSlopeB * link.LinkSpeed, link.IdleSlopeB - link.SendSlopeB);
        var burst = rate * (new Rational(maxFrameSizeNsr + maxFrameSizeA, link.LinkSpeed)
                            - new Rational(maxFrameSizeN * link.IdleSlopeA, link.LinkSpeed * link.SendSlopeA)
                            - new Rational(maxFrameSizeB * link.SendSlopeB, link.LinkSpeed * link.IdleSlopeB));
        return Expressions.FromCurve(new SigmaRhoArrivalCurve(burst, rate), $"shaperB{link.Name}");
    }


    public static CurveExpression MakeMaximalServiceCurveB(Link link) =>
        MakeMaximalServiceCurveB(link, link.Cbs.MaxFrameSizeB);

    public static CurveExpression MakeMaximalServiceCurveB(Link link, BigInteger maxFrameSizeB)
        //Theorem 10
    {
        var rate = new Rational(link.IdleSlopeB * link.LinkSpeed, link.IdleSlopeB - link.SendSlopeB);
        var burst = rate * -new Rational(maxFrameSizeB * link.SendSlopeB, link.LinkSpeed * link.IdleSlopeB);
        return Expressions.FromCurve(new SigmaRhoArrivalCurve(burst, rate), $"maxscB{link.Name}");
    }
}