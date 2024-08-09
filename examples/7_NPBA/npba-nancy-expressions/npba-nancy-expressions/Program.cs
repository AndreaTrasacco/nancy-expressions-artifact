using System.Diagnostics;
using CBS;
using Unipi.Nancy.Expressions.ExpressionsUtility;
using Unipi.Nancy.Numerics;

bool quiet = false;
bool debugACs = false;

foreach(var arg in args)
{
    if(arg == "--quiet")
        quiet = true;

    if(arg == "--debug-ACs")
        debugACs = true;
}

RunCaseStudy();

void RunCaseStudy()
{
    var stopwatch = Stopwatch.StartNew();

    const int maxFrameSizeA = 64;
    const int maxFrameSizeB = 1522;
    const int maxFrameSizeNsr = 1522;
    var cbs = new CreditBasedShaper(maxFrameSizeA, maxFrameSizeB, maxFrameSizeNsr, false);
    const int linkSpeed = 100_000_000;
    var link1 = new Link(cbs, "FC-FS", "FC", "FS", linkSpeed, 65_000_000, -35_000_000, 0, 0);
    var link2 = new Link(cbs, "FS-BS", "FS", "BS", linkSpeed, 65_000_000, -35_000_000, 0, 0);
    var link3 = new Link(cbs, "HU-BS", "HU", "BS", linkSpeed, 75_000_000, -25_000_000, 0, 0);
    var link4 = new Link(cbs, "BS-RU", "BS", "RU", linkSpeed, 75_000_000, -25_000_000, 0, 0);

    var path1 = new[] { link1, link2, link4 };
    var path2 = new[] { link3, link4 };

    var isPeriodic = true;

    // Don't know what it actually means, but it makes UA ACs if false, UPP if true.
    var isWorstCase = false; 

    // the second-to-last parameter is linked to the AC period.
    // making them largely different makes the computations harder
    var flows = new List<Flow>
    {
        new Flow("Flow0", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(5), 10),
        new Flow("Flow1", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow2", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow3", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow4", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow5", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow6", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow7", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow8", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow9", path1, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow10", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow11", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow12", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow13", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow14", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow15", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow16", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow17", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow18", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10),
        new Flow("Flow19", path2, AvbClass.A, isPeriodic, isWorstCase, 64, new Rational(1, 100), 10)
    };
    cbs.SetFlows(flows);

    if(debugACs)
    {
        foreach(var f in flows)
        {
            var ac_expr = f.ComputeSourceAc();
            Console.WriteLine(ac_expr);
            var ac = f.ComputeSourceAc().Compute();
            Console.WriteLine($"{ac.IsUltimatelyAffine} {ac.PseudoPeriodLength}");
        }
    }
    
    var endToEndExpression = cbs.Flows[0].ComputeEndToEndDelay();
    if(!quiet) LatexRenderer.ShowInBrowser(endToEndExpression.ToLatexString(3));
    var endToEnd = endToEndExpression.Compute();
    var endToEndNano = (endToEnd * 1000000000).Ceil();
    if(!quiet) Console.WriteLine("End-To-End Delay:\t" + endToEnd + " s");
    if(!quiet) Console.WriteLine("\t\t\t" + endToEndNano / 1000 + " μs");

    var linkDelayExpression = link4.ComputeDelay(AvbClass.A);
    //if(!quiet) LatexRenderer.ShowInBrowser(linkDelayExpression.ToLatexString(2));
    var linkDelay = linkDelayExpression.Compute();
    var linkDelayNano = (linkDelay * 1000000000).Ceil();
    if(!quiet) Console.WriteLine("Link Delay Class A:\t" + linkDelay + " s");
    if(!quiet) Console.WriteLine("\t\t\t" + linkDelayNano / 1000 + " μs");


    var linkBacklogExpression = link4.ComputeBacklog(AvbClass.A);
    //if(!quiet) LatexRenderer.ShowInBrowser(linkBacklogExpression.ToLatexString(2));
    var linkBacklog = linkBacklogExpression.Compute();
    if(!quiet) Console.WriteLine("Link Backlog Class A:\t" + linkBacklog + " bit");
    if(!quiet) Console.WriteLine("\t\t\t" + linkBacklog.Ceil() + " bit");

    stopwatch.Stop();
    if(!quiet) Console.WriteLine();
    Console.WriteLine($"Total execution time: {stopwatch.Elapsed}");
}