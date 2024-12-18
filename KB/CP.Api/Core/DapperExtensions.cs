﻿using Dapper;

namespace CP.Api.Core;

public static class DapperExtensions
{
    public static IEnumerable<TFirst> Map<TFirst, TSecond, TThird, TKey>
    (
        this SqlMapper.GridReader reader,
        Func<TFirst, TKey> firstKey,
        Func<TSecond, TKey> secondKey,
        Func<TThird, TKey> thirdKey,
        Action<TFirst, IEnumerable<TSecond>> addChildren,
        Action<TFirst, IEnumerable<TThird>> addThirdChildren
    ) where TKey : notnull
    {
        var first = reader.Read<TFirst>().ToList();
        var childMap = reader
            .Read<TSecond>()
            .GroupBy(secondKey)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());

        var childMapThird = reader
            .Read<TThird>()
            .GroupBy(thirdKey)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());

        foreach (var item in first)
        {
            if (childMap.TryGetValue(firstKey(item), out var children))
            {
                addChildren(item, children);
            }

            if (childMapThird.TryGetValue(firstKey(item), out var otherChildren))
            {
                addThirdChildren(item, otherChildren);
            }
        }

        return first;
    }

    public static IEnumerable<TFirst> Map<TFirst, TSecond, TKey>(this SqlMapper.GridReader reader,
        Func<TFirst, TKey> firstKey,
        Func<TSecond, TKey> secondKey,
        Action<TFirst, IEnumerable<TSecond>> addChildren) where TKey : notnull
    {
        var first = reader.Read<TFirst>().ToList();

        var childMap = reader
            .Read<TSecond>()
            .GroupBy(secondKey)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());

        foreach (var item in first)
        {
            if (childMap.TryGetValue(firstKey(item), out var children))
            {
                addChildren(item, children);
            }
        }

        return first;
    }
}