using System;
using System.Collections.Generic;

namespace BikeRouteTracker.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach<T, TArg>(this IEnumerable<T> enumerable, Action<T, TArg> action, TArg arg)
        {
            foreach (var item in enumerable)
            {
                action(item, arg);
            }
        }
    }
}
