using System;
using System.Collections.Generic;

namespace Moen
{
    public class DelegatedComparer<T> : IComparer<T>
    {
        readonly Func<T, T, int> r_Comparer;

        public DelegatedComparer(Func<T, T, int> rpComparer)
        {
            if (rpComparer == null)
                throw null;

            r_Comparer = rpComparer;
        }

        public int Compare(T x, T y)
        {
            return r_Comparer(x, y);
        }
    }
}
