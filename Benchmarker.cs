using CPD_Lab_01;
using ScottPlot;
using System.Buffers;
using System.Diagnostics;

public class Benchmarker<T> where T : struct, IComparable<T>
{
    required public int[] Lengths;
    public Arrays<T> Arrays => new Arrays<T>().CreateArrays(Lengths).CreateArrays(Lengths).CreateArrays(Lengths);
    public List<BenchResult> BenchResults = new();
    public Range _range_of_random => 0..Lengths.Length;
    public Range _range_of_ascending => Lengths.Length..(Lengths.Length * 2);
    public Range _range_of_descending => (Lengths.Length * 2)..(Lengths.Length * 3);
    public async ValueTask<TimeSpan> MeasureAsync(params ISortAlgorithm[] sortingAlgorithms)
    {
        var start = Stopwatch.GetTimestamp();
        await Task.WhenAll(sortingAlgorithms.Select(SortAllArraysAsync));
        return Stopwatch.GetElapsedTime(start);
    }
    public async Task<Arrays<T>> SortAllArraysAsync<Sort>(Sort sortAlg)
    where Sort : ISortAlgorithm
    {
        await Task.WhenAll
        (
            Arrays.FillWith(_range_of_random, FillOption.Random)
                .ForEachAsync(
                    _range_of_random,
                    arr => BenchmarkAsync(arr, sortAlg, $"{sortAlg.Name} :: Random", FillOption.Random),
                    BenchResults
            ),
            Arrays.FillWith(_range_of_ascending, FillOption.Ascending)
                .ForEachAsync(
                    _range_of_ascending,
                    arr => BenchmarkAsync(arr, sortAlg, $"{sortAlg.Name} :: Ascending", FillOption.Ascending),
                    BenchResults
            ),
            Arrays.FillWith(_range_of_descending, FillOption.Descending)
                .ForEachAsync(
                    _range_of_descending,
                    arr => BenchmarkAsync(arr, sortAlg, $"{sortAlg.Name} :: Descending", FillOption.Descending),
                    BenchResults
            )
        );

        return Arrays;
    }
    Task<BenchResult> BenchmarkAsync<Sort>(T[] arrray, Sort sortAlg, string Title, FillOption fillOption)
    where Sort : ISortAlgorithm
    {
        var avrageTime = TimeSpan.Zero;
        return Enumerable.Range(0, 10).ToAsyncEnumerable().AsyncParallelForEach(x =>
            Task.Run(() =>
            {
                var copy = arrray.ToArray().AsSpan();
                var start = Stopwatch.GetTimestamp();
                sortAlg.Sort(copy);
                avrageTime += Stopwatch.GetElapsedTime(start);
            })
        ).ContinueWith(_ => new BenchResult
        {
            Title = Title,
            AvrageTime = avrageTime/10,
            Comaprassions = sortAlg.Comparassions,
            Swaps = sortAlg.Swapings,
            Length = arrray.Length,
            FillOption = fillOption
        });
    }
    static bool IsSorted(T[] array)
    {
        var copy = array.ToArray();
        copy.AsSpan().Sort();
        return array.SequenceEqual(copy);
    }
    public void PlotBenchMarks(List<BenchResult> benchResults)
    {
        var group = benchResults.GroupBy(x => x.FillOption);

        var random = group.First(x => x.Key == FillOption.Random).ToArray();
        var ascending = group.First(x => x.Key == FillOption.Ascending).ToArray();
        var descending = group.First(x => x.Key == FillOption.Descending).ToArray();

        var randomByTitle = random.GroupBy(x => x.Title);
        var ascendingByTitle = ascending.GroupBy(x => x.Title);
        var descendingByTitle = descending.GroupBy(x => x.Title);

        Plot(randomByTitle, "Random");
        Plot(ascendingByTitle, "Ascending");
        Plot(descendingByTitle, "Descending");
    }
    void Plot(IEnumerable<IGrouping<string, BenchResult>> benchesByTitle, string plotName)
    {
        var plot = new Plot(1200, 720);
        plot.Title(plotName);
        foreach (var item in benchesByTitle)
        {
            plot.XLabel("Length");
            plot.YLabel("Avrage Time (seconds)");
            var x = item.Select(x => (double)x.Length).ToArray();
            var y = item.Select(x => (double)x.AvrageTime.TotalSeconds).ToArray();

            plot.AddScatter(
                x, y,
                markerSize: 10,
                label: item.Key
            );
        }
        plot.Legend();
        plot.XTicks(benchesByTitle.First().Select(x => (double)x.Length).ToArray(),
                    benchesByTitle.First().Select(x => x.Length.ToString()).ToArray());
        plot.SaveFig(plotName + ".png");
    }
}
