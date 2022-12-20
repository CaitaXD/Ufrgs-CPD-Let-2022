using CPD_Lab_01;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.Json;
using System.Xml;

var bm = new Benchmarker<byte>()
{
    Lengths = new int[] { 100, 500, 750, 1_000, 5_000, 7_500, 10_000 }
};

var time = await bm.MeasureAsync(
    new InsertionSort(SearchKind.Linear),
    new InsertionSort(SearchKind.Binary),
    new ShellSort()
);

Console.WriteLine("Results:" + Environment.NewLine);
foreach (var result in bm.BenchResults.GroupBy(x => x.Title).SelectMany(x => x.OrderBy(x => x.AvrageTime)))
{
    Console.WriteLine(result + Environment.NewLine);
}
Console.WriteLine($"Time: {(time.TotalMilliseconds < 1000 ? time.TotalMilliseconds + "ms" : time.TotalSeconds + "s")}");

bm.PlotBenchMarks(bm.BenchResults);

Console.WriteLine("Press any key to exit...");
Console.Read();
