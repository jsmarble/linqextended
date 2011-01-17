using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public class BatchEnumerable<TSource> : IEnumerable<IEnumerable<TSource>>
    {
        private IEnumerable<IEnumerable<TSource>> batches;

        public BatchEnumerable(IEnumerable<TSource> source, int batchSize)
        {
            this.batches = CreateBatches(source, batchSize);
        }

        private IEnumerable<IEnumerable<TSource>> CreateBatches(IEnumerable<TSource> source, int batchSize)
        {
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    IEnumerable<TSource> batch = CreateBatch(enumerator, batchSize).ToList();
                    yield return batch;
                }
            }
        }

        private IEnumerable<TSource> CreateBatch<TSource>(IEnumerator<TSource> enumerator, int count)
        {
            int countInternal = 1;
            yield return enumerator.Current;
            while (countInternal < count && enumerator.MoveNext())
            {
                countInternal++;
                yield return enumerator.Current;
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<IEnumerable<TSource>> GetEnumerator()
        {
            return this.batches.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
