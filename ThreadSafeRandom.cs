using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CPD_Lab_01;

/// <summary>
/// Generates random number using a global and local random number generator
/// </summary>
public static class ThreadSafeRandom
{
    private static readonly Random _global = new();
    [ThreadStatic] private static Random? _local;
    public static int Next()
    {
        if (_local == null)
        {
            int seed;
            lock (_global)
            {
                seed = _global.Next();
            }
            _local = new Random(seed);
        }
        return _local.Next();
    }
    // NextBytes 
    public static void NextBytes(Span<byte> buffer)
    {
        if (_local == null)
        {
            int seed;
            lock (_global)
            {
                seed = _global.Next();
            }
            _local = new Random(seed);
        }
        _local.NextBytes(buffer);
    }
    public static void NextBytes(byte[] buffer)
    {
        if (_local == null)
        {
            int seed;
            lock (_global)
            {
                seed = _global.Next();
            }
            _local = new Random(seed);
        }
        _local.NextBytes(buffer);
    }
}
