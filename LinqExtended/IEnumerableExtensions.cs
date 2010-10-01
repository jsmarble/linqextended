using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqExtended
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains at least a given number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence for which to check the count.</param>
        /// <param name="count">The minimum count of items.</param>
        /// <returns>true if the sequence contains no fewer than the given number of elements; otherwise, false.</returns>
        public static bool ContainsAtLeast<T>(this IEnumerable<T> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            int actualCount = 0;
            IEnumerator<T> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                actualCount++;
                if (actualCount >= count)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a sequence contains at most a given number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence for which to check the count.</param>
        /// <param name="count">The maximum count of items.</param>
        /// <returns>true if the sequence contains no more than the given number of elements; otherwise, false.</returns>
        public static bool ContainsAtMost<T>(this IEnumerable<T> source, int count)
        {
            source.Contains(default(T));
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            int actualCount = 0;
            IEnumerator<T> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                actualCount++;
                if (actualCount > count)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Performs the specified action on each element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence containing items to be passed when invoking the action.</param>
        /// <param name="action">The action to invoke for each item.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}
