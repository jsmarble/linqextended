using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Extended
{
    public static class IEnumerableExtensions
    {
        private static Random random = new Random();

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

            int takeCount = source.Take(count).Count(); //Take method will take as many as exist up to the count.
            return takeCount == count; //If the take count was less than the count param then the source did not contain at least [count] elements.
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

            int takeCount = source.Take(count + 1).Count(); //Take method will take as many as exist up to the count. Take count+1 so we know if more existed than [count].
            return takeCount <= count; //If the take count was less than or equal to the count param then the source did not contain more than [count] elements.
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
                action(item);
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
            EqualityComparison<TSource> equalityComparison = (x, y) => keySelector(x).CompareTo(keySelector(y)) == 0;
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
            IEqualityComparer<TSource> comparer = new EqualityComparisonAdapter<TSource>(equalityComparison);
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
            return source.OrderBy(x => x, new ComparisonAdapter<TSource>(comparison));
        }

        #endregion

        #region ThenBy

        /// <summary>
        /// Performs a subsequent ordering in a sequence in ascending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="System.Collections.Generic.IComparer{T}"/> to compare elements.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparer.</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.ThenBy(x => x, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering in a sequence in ascending order by using a specified comparison.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="System.Comparison{T}"/> to use to compare the elements in the collection.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparison.</returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            return source.ThenBy(x => x, new ComparisonAdapter<TSource>(comparison));
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
            return source.OrderByDescending(x => x, new ComparisonAdapter<TSource>(comparison));
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
            return source.OrderByDescending((x, y) =>
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
            });
        }

        #endregion

        #region ThenByDescending

        /// <summary>
        /// Performs a subsequent ordering in a sequence in descending order by using a specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="System.Collections.Generic.IComparer{T}"/> to compare elements.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparer.</returns>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource>(this IOrderedEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.ThenByDescending(x => x, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering in a sequence in descending order by using a specified comparison.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="System.Comparison{T}"/> to use to compare the elements in the collection.</param>
        /// <returns>An <see cref="System.Linq.IOrderedEnumerable{T}"/> whose elements are sorted according to a comparison.</returns>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource>(this IOrderedEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            return source.ThenByDescending(x => x, new ComparisonAdapter<TSource>(comparison));
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Returns a collection of random elements from a sequence. Note that the nature of randomizing the sequence requires enumerating the entire sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence from which to return the random elements.</param>
        /// <returns>a <see cref="System.Collections.Generic.IEnumerable{T}"/> containing elements from the sequence.</returns>
        /// <remarks>The results of this function will not contain duplicate elements.</remarks>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            List<TSource> list = source.ToList();
            if (list.Count > 1)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    TSource tmp = list[i];
                    int randomIndex = random.Next(i + 1);

                    //Swap elements
                    list[i] = list[randomIndex];
                    list[randomIndex] = tmp;
                }
            }
            return list;
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
            return values.Any(x => source.Contains(x));
        }

        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            return values.Any(x => source.Contains(x, equalityComparer));
        }

        #endregion

        #region ContainsAll

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.All(x => source.Contains(x));
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            return values.All(x => source.Contains(x, equalityComparer));
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

        #region Batch

        /// <summary>
        /// Creates batches from the enumerable source according to the specified batch size. The source will be enumerated completely for each batch.
        /// </summary>
        /// <typeparam name="TSource">An <see cref="System.Collections.Generic.IEnumerable{T}"/> to batch.</typeparam>
        /// <param name="source">The sequence to batch.</param>
        /// <param name="batchSize">The size of the batches.</param>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerable{T}"/> that contains batches from the input sequence.</returns>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int batchSize)
        {
            return new BatchEnumerable<TSource>(source, batchSize);
        }

        #endregion
    }
}
