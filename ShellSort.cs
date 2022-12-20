namespace CPD_Lab_01;

public struct ShellSort : ISortAlgorithm
{
    public void Clear()
    {
        Comparassions = 0;
        Swapings = 0;
    }
    public ShellSort()
    {
        Name = "ShellSort";
    }
    public ulong Comparassions { get; private set; }
    public ulong Swapings { get; private set; }
    public IEnumerable<int>? GapSequence { get; private set; }

    public string Name { get; }
    public ShellSort(IEnumerable<int> gapSequence, string name = "ShellSort")
    {
        GapSequence = gapSequence;
        Name = name;
    }
    public void Sort<T>(Span<T> span) where T : IComparable<T>
    {
        if (GapSequence is null)
        {
            _Sort(span, log_length(span.Length));
            IEnumerable<int> log_length(int length)
            {
                for (var gap = length / 2; gap > 0; gap /= 2)
                    yield return gap;
            }
            return;
        }
        else
        {
            _Sort(span, GapSequence);
        }
    }
    void _Sort<T>(Span<T> span, IEnumerable<int> gapSequence) where T : IComparable<T>
    {
        foreach (var gap in gapSequence)
        {
            Comparassions++;
            for (int i = gap; i < span.Length; i++)
            {
                Comparassions++;
                var temp = span[i];
                int j = i;
                while (j >= gap && span[j - gap].CompareTo(temp) > 0)
                {
                    Comparassions += 2; // && is Branching
                    span[j] = span[j - gap];
                    Swapings++;
                    j -= gap;
                }
                Swapings++;
                span[j] = temp;
            }
        }
    }
}