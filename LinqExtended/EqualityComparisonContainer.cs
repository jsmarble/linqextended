using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class EqualityComparisonContainer<T> : IEqualityComparer<T>
    {
        private EqualityComparison<T> equalityComparison;

        public EqualityComparisonContainer(EqualityComparison<T> equalityComparison)
        {
            if (equalityComparison == null) throw new ArgumentNullException("equalityComparison");
            this.equalityComparison = equalityComparison;
        }

        #region Public Methods

        public bool Equals(T x, T y)
        {
            return this.equalityComparison(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }

        #endregion
    }
}
