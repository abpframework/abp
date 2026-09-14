using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp;

/// <summary>
/// A shortcut to use <see cref="Random"/> class.
/// Also provides some useful methods.
/// </summary>
public static class RandomHelper
{
    private readonly static Random Rnd = new Random();

    /// <summary>
    /// Returns a random number within a specified range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to minValue and less than maxValue;
    /// that is, the range of return values includes minValue but not maxValue.
    /// If minValue equals maxValue, minValue is returned.
    /// </returns>
    public static int GetRandom(int minValue, int maxValue)
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        lock (Rnd)
        {
            return Rnd.Next(minValue, maxValue);
        }
#else
        return Random.Shared.Next(minValue, maxValue);
#endif
    }

    /// <summary>
    /// Returns a nonnegative random number less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to zero, and less than maxValue;
    /// that is, the range of return values ordinarily includes zero but not maxValue.
    /// However, if maxValue equals zero, maxValue is returned.
    /// </returns>
    public static int GetRandom(int maxValue)
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        lock (Rnd)
        {
            return Rnd.Next(maxValue);
        }
#else
        return Random.Shared.Next(maxValue);
#endif
    }

    /// <summary>
    /// Returns a nonnegative random number.
    /// </summary>
    /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.</returns>
    public static int GetRandom()
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        lock (Rnd)
        {
            return Rnd.Next();
        }
#else
        return Random.Shared.Next();
#endif
    }

    /// <summary>
    /// Gets random of given objects.
    /// </summary>
    /// <typeparam name="T">Type of the objects</typeparam>
    /// <param name="objs">List of object to select a random one</param>
    public static T GetRandomOf<T>([NotNull] params T[] objs)
    {
        Check.NotNullOrEmpty(objs, nameof(objs));

        return objs[GetRandom(0, objs.Length)];
    }

    /// <summary>
    /// Gets random item from the given list.
    /// </summary>
    /// <typeparam name="T">Type of the objects</typeparam>
    /// <param name="list">List of object to select a random one</param>
    public static T GetRandomOfList<T>([NotNull] IList<T> list)
    {
        Check.NotNullOrEmpty(list, nameof(list));

        return list[GetRandom(0, list.Count)];
    }

    /// <summary>
    /// Generates a randomized list from given enumerable.
    /// </summary>
    /// <typeparam name="T">Type of items in the list</typeparam>
    /// <param name="items">items</param>
    public static List<T> GenerateRandomizedList<T>([NotNull] IEnumerable<T> items)
    {
        var array = items.ToArray();
#if NETSTANDARD2_0 || NETSTANDARD2_1
        lock (Rnd)
        {
            return array.Shuffle(Rnd).ToList();
        }
#else
        Random.Shared.Shuffle(array);
        return array.ToList();
#endif
    }
}
