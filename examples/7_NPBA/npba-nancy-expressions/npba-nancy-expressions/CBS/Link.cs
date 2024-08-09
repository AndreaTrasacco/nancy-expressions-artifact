using System.Data;
using System.Linq.Expressions;
using System.Numerics;
using Unipi.Nancy.Expressions;
using Unipi.Nancy.MinPlusAlgebra;
using Unipi.Nancy.NetworkCalculus;
using Unipi.Nancy.Numerics;

namespace CBS;

public record Link(
    CreditBasedShaper Cbs,
    string Name,
    string NodeI,
    string NodeJ,
    BigInteger LinkSpeed,
    BigInteger IdleSlopeA,
    BigInteger SendSlopeA,
    BigInteger IdleSlopeB,
    BigInteger SendSlopeB)
{
    public RationalExpression? ComputedDelayA { get; private set; }
    public RationalExpression? ComputedDelayB { get; private set; }
    public CurveExpression? OutgoingArrivalCurveA { get; private set; }
    public CurveExpression? OutgoingArrivalCurveB { get; private set; }


    public CurveExpression ComputeIncomingArrivalCurve(AvbClass avbClass)
    {
        var startingFlowsACs = StartingFlows(avbClass)
            .Select(flow => flow.ComputeSourceAc().WithName("SourceAc" + flow.Name));
        var previousLinksACs = PreviousLinks()
            .Select(link => link.ComputeOutgoingArrivalCurve(avbClass).WithName("OutgoingAc" + link.Name));

        var accumulatedCurve = Enumerable.Concat(startingFlowsACs, previousLinksACs)
            .Sum().WithName("accCurveLink" + Name);

        return accumulatedCurve;
    }

    public CurveExpression ComputeOutgoingArrivalCurve(AvbClass avbClass)
    {
        switch (avbClass)
        {
            case AvbClass.A when OutgoingArrivalCurveA != null:
                return OutgoingArrivalCurveA;
            case AvbClass.B when OutgoingArrivalCurveB != null:
                return OutgoingArrivalCurveB;
            case AvbClass.Nsr:
            default:
                return _ComputeOutgoingArrivalCurve(avbClass);
        }
    }

    private CurveExpression _ComputeOutgoingArrivalCurve(AvbClass avbClass)
    {
        var acIn = ComputeIncomingArrivalCurve(avbClass);
        var maxServiceCurve = ComputeMaximalServiceCurve(avbClass);
        var minServiceCurve = ComputeMinimalServiceCurve(avbClass);
        var shaper = ComputeShapingCurve(avbClass);

        var curveConvoluted = acIn.Convolution(maxServiceCurve).Deconvolution(minServiceCurve);
        var shapedCurve = shaper.Minimum(curveConvoluted);

        if (avbClass == AvbClass.A)
            OutgoingArrivalCurveA = shapedCurve;
        if (avbClass == AvbClass.B)
            OutgoingArrivalCurveB = shapedCurve;
        return shapedCurve;
    }

    public CurveExpression ComputeShapingCurve(AvbClass avbClass)
    {
        switch (avbClass)
        {
            case AvbClass.A:
                return CreditBasedShaper.MakeShaperCurveA(this);
            case AvbClass.B:
                return CreditBasedShaper.MakeShaperCurveB(this);
            case AvbClass.Nsr:
            default:
                throw new ArgumentException("Invalid AVB Class:" + avbClass);
        }
    }

    public CurveExpression ComputeMaximalServiceCurve(AvbClass avbClass)
    {
        switch (avbClass)
        {
            case AvbClass.A:
                return CreditBasedShaper.MakeMaximalServiceCurveA(this);
            case AvbClass.B:
                return CreditBasedShaper.MakeMaximalServiceCurveB(this);
            case AvbClass.Nsr:
            default:
                throw new ArgumentException("Invalid AVB Class:" + avbClass);
        }
    }

    public CurveExpression ComputeMinimalServiceCurve(AvbClass avbClass) =>
        ComputeMinimalServiceCurve(avbClass, Cbs.IsStrict);

    public CurveExpression ComputeMinimalServiceCurve(AvbClass avbClass, bool isStrict)
    {
        if (avbClass == AvbClass.A)
        {
            return isStrict
                ? CreditBasedShaper.MakeStrictMinimalServiceCurveA(this, Cbs.MaxFrameSizeA, Cbs.MaxFrameSizeN)
                : CreditBasedShaper.MakeMinimalServiceCurveA(this, Cbs.MaxFrameSizeN);
        }

        return isStrict
            ? CreditBasedShaper.MakeStrictMinimalServiceCurveB(this, Cbs.MaxFrameSizeA, Cbs.MaxFrameSizeB,
                Cbs.MaxFrameSizeNsr)
            : CreditBasedShaper.MakeMinimalServiceCurveB(this, Cbs.MaxFrameSizeA, Cbs.MaxFrameSizeNsr,
                Cbs.MaxFrameSizeN);
    }

    public RationalExpression ComputeBacklog(AvbClass avbClass)
    {
        var minimalService = ComputeMinimalServiceCurve(avbClass);
        var arrivalCurve = ComputeIncomingArrivalCurve(avbClass);
        return Expressions.VerticalDeviation(arrivalCurve, minimalService);
    }

    public RationalExpression ComputeDelay(AvbClass avbClass)
    {
        switch (avbClass)
        {
            case AvbClass.A when ComputedDelayA != null:
                return ComputedDelayA;
            case AvbClass.B when ComputedDelayB != null:
                return ComputedDelayB;
            case AvbClass.Nsr:
                throw new ArgumentException("Delay of NSR can not be computed.");
            default:
                return _ComputeDelay(avbClass);
        }
    }

    private RationalExpression _ComputeDelay(AvbClass avbClass)
    {
        var minimalService = ComputeMinimalServiceCurve(avbClass);
        var arrivalCurve = ComputeIncomingArrivalCurve(avbClass);
        var delay = Expressions.HorizontalDeviation(arrivalCurve, minimalService
            , expressionName: "delayLink" + Name);

        if (avbClass == AvbClass.A)
            ComputedDelayA = delay.WithName(delay.Name + "A");
        if (avbClass == AvbClass.B)
            ComputedDelayB = delay.WithName(delay.Name + "B");
        return delay;
    }

    private List<Flow> PassingFlows() => Cbs.Flows.Where(flow => flow.Path.Contains(this)).ToList();

    private List<Link> PreviousLinks()
    {
        var prevLinks = new HashSet<Link>();
        var flows = PassingFlows();
        foreach (var flow in flows)
        {
            var index = Array.IndexOf(flow.Path, this);
            if (index > 0)
            {
                prevLinks.Add(flow.Path[index - 1]);
            }
        }

        return prevLinks.ToList();
    }

    private List<Flow> StartingFlows(AvbClass avbClass) =>
        Cbs.Flows.Where(flow => flow.Path[0] == this && flow.AvbClass == avbClass).ToList();
}