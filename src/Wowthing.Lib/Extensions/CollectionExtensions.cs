﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Wowthing.Lib.Utilities;

namespace Wowthing.Lib.Extensions
{
    public static class CollectionExtensions
    {
        public static Dictionary<TKey, TValue> EmptyIfNull<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict ?? new Dictionary<TKey, TValue>();
        }

        public static List<T> EmptyIfNull<T>(this List<T> list)
        {
            return list ?? new List<T>();
        }

        public static T[] EmptyIfNull<T>(this T[] array)
        {
            return array ?? Array.Empty<T>();
        }
        
        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            if (!result.Any())
            {
                return result;
            }
            return result.Concat(result.SelectManyRecursive(selector));
        }

        public static string ToPackedUInt16Array(this List<int> list)
        {
            return SerializationUtilities.SerializeUInt16Array(list.Select(id => Convert.ToUInt16(id)).ToArray());
        }

        public static Dictionary<TKey, TObject[]> ToGroupedDictionary<TObject, TKey>(
            this ICollection<TObject> things,
            Func<TObject, TKey> keyFunc
        )
        {
            return things
                .GroupBy(keyFunc)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToArray()
                );
        }

        public static Dictionary<TKey, TReturn[]> ToGroupedDictionary<TObject, TKey, TReturn>(
            this ICollection<TObject> things,
            Func<TObject, TKey> keyFunc, 
            Func<TObject, TReturn> valueFunc
        )
        {
            return things
                .GroupBy(keyFunc)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(valueFunc)
                        .ToArray()
                );
        }
    }
}
