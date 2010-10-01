using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqExtended
{
    public static class IEnumerableExtensions
    {
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

        public static bool ContainsAtMost<T>(this IEnumerable<T> source, int count)
        {
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
    }
}
