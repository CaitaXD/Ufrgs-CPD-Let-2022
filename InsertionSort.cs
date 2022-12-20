using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CPD_Lab_01;

public enum SearchKind
{
    Linear = 0 << 0,
    Binary = 1 << 0,
}
public interface ISortAlgorithm
{
    public string Name { get; }
    public ulong Comparassions { get; }
    public ulong Swapings { get; }
    public void Sort<T>(Span<T> span) where T : IComparable<T>;

    void Clear();
}

public struct InsertionSort : ISortAlgorithm
{
    public void Clear()
    {
        Comparassions = 0;
        Swapings = 0;
    }
    public InsertionSort(SearchKind searchAlgorithm)
    {
        SearchAlgorithm = searchAlgorithm;
        Name = searchAlgorithm is SearchKind.Linear ? "InsertionSort" : "InsertionSort (Binary)";
    }
    public string Name { get; }
    public SearchKind SearchAlgorithm { get; private set; }
    public ulong Comparassions { get; private set; }
    public ulong Swapings { get; private set; }
    public void Sort<T>(Span<T> span)
        where T : IComparable<T>
    {
        switch (SearchAlgorithm)
        {
        case SearchKind.Linear:
        _InsertionSort(span);
        return;

        case SearchKind.Binary:
        for (int i = 1; i < span.Length; ++i)
        {
            Comparassions++;
            var selected = span[i];
            var index = BinarySearch(span[..i], selected);
            index = index < 0 ? ~index : index;
            for (var j = i - 1; j >= index; --j)
            {
                Comparassions++;
                span[j + 1] = span[j];
                Swapings++;
            }
            span[index] = selected;
            Swapings++;
        }
        return;
        }
    }
    void _InsertionSort<T>(Span<T> span)
        where T : IComparable<T>
    {
        for (var i = 0; i < span.Length; ++i)
        {
            Comparassions++;
            var selected = span[i];
            var j = i - 1;
            for (; j >= 0 && span[j].CompareTo(selected) > 0; --j)
            {
                Comparassions += 2;  // && is Branching
                Swapings++;
                span[j + 1] = span[j];
            }
            Swapings++;
            span[j + 1] = selected;
        }
    }
    int BinarySearch<T>(Span<T> span, T key) where T : IComparable<T>
    {
        int left = 0, right = span.Length - 1, mid = 0;
        while (left <= right & span[mid].CompareTo(key) != 0)
        {
            Comparassions++;
            mid = (left + right) / 2;
            int compare_result = span[mid].CompareTo(key);
            left = (Convert.ToInt32(compare_result < 0) * (mid + 1)) | (Convert.ToInt32(compare_result > 0) * left);
            right = (Convert.ToInt32(compare_result > 0) * (mid - 1)) | (Convert.ToInt32(compare_result < 0) * right);
        }
        if (span[mid].CompareTo(key) == 0)
        {
            Comparassions++;
            return mid;
        }
        return ~left;
    }
};
