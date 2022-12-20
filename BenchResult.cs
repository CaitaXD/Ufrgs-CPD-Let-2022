using CPD_Lab_01;

public class BenchResult
{
    required public string Title { get; init; }
    required public int Length { get; init; }
    required public TimeSpan AvrageTime { get; init; }
    required public ulong Comaprassions { get; init; }
    public ulong Swaps { get; init; }
    public FillOption FillOption { get; init; }


    public override string ToString()
    {
        return
            $"""
            {Title}
            Lenght: {Length}
            Time: {(AvrageTime.TotalMilliseconds < 1000 ? AvrageTime.TotalMilliseconds.ToString() + "ms" : AvrageTime.TotalSeconds.ToString() + "s")}
            Comparission: {Comaprassions}
            {(Swaps > 0 ? $"Swaps: {Swaps}" : "")}
            """;
    }
}
