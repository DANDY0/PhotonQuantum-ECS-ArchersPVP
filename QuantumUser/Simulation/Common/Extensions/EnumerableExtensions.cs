using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            if (enumerable is ICollection<T> collection)
                return collection.Count == 0;

            return !enumerable.Any();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static T PickRandom<T>(this IEnumerable<T> collection)
        {
            var enumerable = collection as T[] ?? collection.ToArray();
            return enumerable[Random.Range(0, enumerable.Length)];
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T toExcept)
        {
            return enumerable.Except(new[] { toExcept });
        }

        public static T ElementAtOrFirst<T>(this T[] array, int index)
        {
            return index < array.Length ? array[index] : array[0];
        }

        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> self)
        {
            return self ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> NoNulls<T>(this IEnumerable<T> self)
        {
            return self.Where(element => element != null);
        }
    }
}