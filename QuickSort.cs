using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPD_Lab_01;
internal struct QuickSort : ISortAlgorithm
{
    public string Name => "Qucik Sort";

    public ulong Comparassions { get; private set; }

    public ulong Swapings { get; private set; }

    public void Clear()
    {
        Comparassions = 0;
        Swapings = 0;
    }
    public void Sort<T>(Span<T> span) where T : IComparable<T>
    {
        // Implement Quick Sort


    }
}
