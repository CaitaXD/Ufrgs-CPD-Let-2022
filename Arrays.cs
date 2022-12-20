using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPD_Lab_01;
public enum FillOption
{
    Random,
    Ascending,
    Descending,
    Zero
}
public class Arrays<T> where T : IComparable<T>
{
    public int CombinedLength => _arrays.Max(x => x.Length);
    public int Length => _arrays.Count;
    internal List<T[]> _arrays = new();
    public T[] this[int index]
    {
        get => _arrays[index];
    }
    public Arrays<T> CreateArrays(params int[] lengths)
    {
        foreach (int length in lengths.AsSpan())
        {
            _arrays.Add(new T[length]);
        }
        return this;
    }
    public Task<Arrays<T>> ForEachAsync<R>(Range range, Func<T[], Task<R>> func, IList<R> resultAccumulator)
    {
        var ListOfTasks = new Task<R>[range.End.Value - range.Start.Value];
        
        for (var i = range.Start.Value; i < range.End.Value; ++i)
        {
            var array = _arrays[i];
            ListOfTasks[i - range.Start.Value] = func(array);
        }

        return Task.WhenAll(ListOfTasks).ContinueWith(results =>
        {
            foreach (var result in results.Result)
            {
                resultAccumulator.Add(result);
            }
            return this;
        });
    }
}
public static class ArraysExtentions
{
    public static Arrays<T> FillWith<T>(this Arrays<T> arrays, FillOption option) where T : struct, IComparable<T>
    {
        if (option is FillOption.Random)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays))
            {
                var as_bytes = MemoryMarshal.AsBytes(array.AsSpan());
                ThreadSafeRandom.NextBytes(as_bytes);
            }
        }
        else if (option is FillOption.Zero)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays))
            {
                array.AsSpan().Clear();
            }
        }
        else if (option is FillOption.Ascending)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays))
            {
                for (var i = 0; i < array.Length; ++i)
                {
                    var value = MemoryMarshal.Read<T>(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref i, 1)));
                    array[i] = value;
                }
            }
        }
        else if (option is FillOption.Descending)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays))
            {
                for (var i = 0; i < array.Length; ++i)
                {
                    var reverse = array.Length - i;
                    var value = MemoryMarshal.Read<T>(
                        MemoryMarshal.AsBytes(
                            MemoryMarshal.CreateSpan(ref reverse, 1)));
                    array[i] = value;
                }
            }
        }
        return arrays;
    }
    public static Arrays<T> FillWith<T>(this Arrays<T> arrays, Range range, FillOption option) where T : struct, IComparable<T>
    {
        if (option is FillOption.Random)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays)[range])
            {
                var as_bytes = MemoryMarshal.AsBytes(array.AsSpan());
                ThreadSafeRandom.NextBytes(as_bytes);
            }
        }
        else if (option is FillOption.Zero)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays)[range])
            {
                array.AsSpan().Clear();
            }
        }
        else if (option is FillOption.Ascending)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays)[range])
            {
                for (var i = 0; i < array.Length; ++i)
                {
                    var value = MemoryMarshal.Read<T>(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref i, 1)));
                    array[i] = value;
                }
            }
        }
        else if (option is FillOption.Descending)
        {
            foreach (var array in CollectionsMarshal.AsSpan(arrays._arrays)[range])
            {
                for (var i = 0; i < array.Length; ++i)
                {
                    var reverse = array.Length - i;
                    var value = MemoryMarshal.Read<T>(
                        MemoryMarshal.AsBytes(
                            MemoryMarshal.CreateSpan(ref reverse, 1)));
                    array[i] = value;
                }
            }
        }
        return arrays;
    }
}
