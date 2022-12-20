using System.Diagnostics;
using System.Text;
using CPD_Lab_01;

public static class Pretty
{
    public static string? PrettyPrint<T>(T[] arr)
    {
        StringBuilder stringBuilder = new();
        int n = (int)Math.Ceiling(Math.Sqrt(arr.Length));
        int max_len = arr.Max(x => $"{x}".Length);
        for (int i = 0; i < n; i++)
        {
            stringBuilder.Append('|');
            for (int j = 0; j < n; j++)
            {
                int index = i * n + j;
                if (index < arr.Length)
                {
                    stringBuilder.Append($"{arr[index]}".PadLeft(max_len, '0'));
                    if (j < n - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }
            }
            stringBuilder.AppendLine("|");
        }
        return stringBuilder.ToString();
    }
    public static string BenchResultsString(BenchResult[]? benchResults)
    {
        return string.Join(Environment.NewLine, benchResults!.Select(x => $"{x}\n"));
    }
    public static BenchResult BenchMarkResult<T, TSort>(
        T[] array,
        TSort sortingAlgorithm,
        string Title
    )
        where T : IComparable<T>
        where TSort : ISortAlgorithm
    {
        var start = Stopwatch.GetTimestamp();
        sortingAlgorithm.Sort(array.AsSpan());
        var time = Stopwatch.GetElapsedTime(start);

        return new()
        {
            Title = Title,
            Length = array.Length,
            Comaprassions = sortingAlgorithm.Comparassions,
            Swaps = sortingAlgorithm.Swapings,
            AvrageTime = time,
        };
    }
}
