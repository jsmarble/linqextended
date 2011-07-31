using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    public class GuidEnumerable : IEnumerable<Guid>
    {
        public int EnumeratedCount { get; private set; }

        public IEnumerator<Guid> GetEnumerator()
        {
            while (true)
            {
                EnumeratedCount++;
                yield return Guid.NewGuid();
            }
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
