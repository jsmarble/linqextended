using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Extended
{
    public static class IEnumerableExtensions
    {
        #region ContainsAt[Least|Most]
        /// <summary>
        /// Determines whether a sequence contains at least a given number of elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence for which to check the count.</param>
        /// <param name="count">The minimum count of items.</param>
        /// <returns>true if the sequence contains no fewer than the given number of elements; otherwise, false.</returns>
        public static bool ContainsAtLeast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            int actualCount = 0;
            IEnumerator<TSource> enumerator = source.GetEnumerator();
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
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence for which to check the count.</param>
        /// <param name="count">The maximum count of items.</param>
        /// <returns>true if the sequence contains no more than the given number of elements; otherwise, false.</returns>
        public static bool ContainsAtMost<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            int actualCount = 0;
            IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                actualCount++;
                if (actualCount > count)
                    return false;
            }
            return true;
        }
        #endregion

        #region ForEach

        /// <summary>
        /// Performs the specified action on each element in the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence containing items to be passed when invoking the action.</param>
        /// <param name="action">The <see cref="System.Action{T}"/> to invoke for each item.</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source)
            {
                action(item);
            }
        }

        #endregion

        #region Distinct

        /// <summary>
        /// Returns distinct elements from a sequence based on an equality key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="keySelector"> A function to extract a key from an element.</param>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, IComparable> keySelector)
        {
            EqualityComparison<TSource> equalityComparison = new EqualityComparison<TSource>((x, y) => keySelector(x).CompareTo(keySelector(y)) == 0);
            return source.Distinct(equalityComparison);
        }

        /// <summary>
        /// Returns distinct elements from a sequence based on an equality comparison.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="equalityComparison">The <see cref="System.EqualityComparison{T}"/> to use to compare the elements in the collection.</param>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, EqualityComparison<TSource> equalityComparison)
        {
            IEqualityComparer<TSource> comparer = new EqualityComparisonContainer<TSource>(equalityComparison);
            return source.Distinct(comparer);
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="System.Collections.Generic.IComparer{T}"/> to compare elements.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparer.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.OrderBy(x => x, comparer);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparison.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="System.Comparison{T}"/> to use to compare the elements in the collection.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparison.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            return source.OrderBy(x => x, new ComparisonContainer<TSource>(comparison));
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a collection of keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelectors">A collection of functions to extract keys from an element.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a collection of keys.</returns>
        public static IOrderedEnumerable<TElement> OrderBy<TElement>(this IEnumerable<TElement> source, params Func<TElement, IComparable>[] keySelectors)
        {
            return source.OrderBy(new Comparison<TElement>((x, y) =>
            {
                foreach (Func<TElement, IComparable> keySelector in keySelectors)
                {
                    IComparable keyX = keySelector(x);
                    IComparable keyY = keySelector(y);
                    int result = keyX.CompareTo(keyY);
                    if (result != 0)
                        return result;
                }
                return 0;
            }));
        }

        #endregion

        #region OrderByDescending

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="System.Collections.Generic.IComparer{T}"/> to compare elements.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparer.</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.OrderByDescending(x => x, comparer);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparison.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="System.Comparison{T}"/> to use to compare the elements in the collection.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparison.</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            return source.OrderByDescending(x => x, new ComparisonContainer<TSource>(comparison));
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a collection of keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelectors">A collection of functions to extract keys from an element.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a collection of keys.</returns>
        public static IOrderedEnumerable<TElement> OrderByDescending<TElement>(this IEnumerable<TElement> source, params Func<TElement, IComparable>[] keySelectors)
        {
            return source.OrderByDescending(new Comparison<TElement>((x, y) =>
            {
                foreach (Func<TElement, IComparable> keySelector in keySelectors)
                {
                    IComparable keyX = keySelector(x);
                    IComparable keyY = keySelector(y);
                    int result = keyX.CompareTo(keyY);
                    if (result != 0)
                        return result;
                }
                return 0;
            }));
        }

        #endregion

        #region Random

        /// <summary>
        /// Returns a random element from a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence from which to return a random element.</param>
        /// <returns>an element from the sequence.</returns>
        public static TSource Random<TSource>(this IEnumerable<TSource> source)
        {
            if (source.Any())
                return source.Random(1).FirstOrDefault();
            else
                return default(TSource);
        }

        /// <summary>
        /// Returns a collection of random elements from a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence from which to return the random elements.</param>
        /// <returns>a <see cref="System.Collections.Generic.IEnumerable{T}"/> containing elements from the sequence.</returns>
        /// <remarks>The results of this function will not contain duplicate elements.</remarks>
        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count)
        {
            if (count > source.Count())
                throw new ArgumentOutOfRangeException("Value cannot be greater than the count of the source collection.");

            List<T> itemPool = source.ToList();
            List<T> result = new List<T>();
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                int index = r.Next(0, itemPool.Count - 1);
                T value = itemPool[index];
                itemPool.Remove(value);
                result.Add(value);
                if (!itemPool.Any())
                    break;
            }
            return result;
        }

        #endregion

        #region IEnumerable<string> Join

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return source.Join(separator, false);
        }

        public static string Join(this IEnumerable<string> source, string separator, bool excludeEmptyItems)
        {
            if (excludeEmptyItems)
                source = source.Except(x => string.IsNullOrEmpty(x));
            return string.Join(separator, source.ToArray());
        }

        #endregion

        #region Append

        /// <summary>
        /// Appends the value to the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to append the value to.</param>
        /// <param name="value">The value to append to the sequence.</param>
        /// <returns>the sequence with the appended value.</returns>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            return source.Concat(new TSource[] { value });
        }

        #endregion

        #region ContainsAny

        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            foreach (T item in values)
            {
                if (source.Contains(item))
                    return true;
            }
            return false;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            foreach (T item in values)
            {
                if (source.Contains(item, equalityComparer))
                    return true;
            }
            return false;
        }

        #endregion

        #region ContainsAll

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            foreach (T item in values)
            {
                if (!source.Contains(item))
                    return false;
            }
            return true;
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            foreach (T item in values)
            {
                if (!source.Contains(item, equalityComparer))
                    return false;
            }
            return true;
        }

        #endregion

        #region Except

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">An <see cref="System.Collections.Generic.IEnumerable{T}"/> to filter.</typeparam>
        /// <param name="source">The sequence to exclude elements from.</param>
        /// <param name="predicate">The predicate to use to determine if an element will be excluded from the result.</param>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerable{T}"/> 
        /// that contains elements from the input sequence that satisfy the condition.</returns>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where(item => !predicate(item));
        }

        /// <summary>
        /// Filters a sequence to exclude the specified element.
        /// </summary>
        /// <typeparam name="TSource">An <see cref="System.Collections.Generic.IEnumerable{T}"/> to filter.</typeparam>
        /// <param name="source">The sequence to exclude elements from.</param>
        /// <param name="item">The element to exclude from the result.</param>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerable{T}"/> 
        /// that contains elements from the input sequence except the specified element.</returns>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            return source.Except(x => (object)x == (object)item);
        }

        #endregion
    }
}
