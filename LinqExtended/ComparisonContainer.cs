﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class ComparisonContainer<T> : IComparer<T>
    {
        private Comparison<T> comparison;

        public ComparisonContainer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        #region IComparer<TSource> Members

        public int Compare(T x, T y)
        {
            return comparison.Invoke(x, y);
        }

        #endregion
    }
}
