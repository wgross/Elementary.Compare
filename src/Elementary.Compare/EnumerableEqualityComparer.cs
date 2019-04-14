using System;
using System.Collections;
using System.Collections.Generic;

namespace Elementary.Compare
{
    public class EnumerableEqualityComparer : IEqualityComparer<IEnumerable>
    {
        public static IEqualityComparer<IEnumerable> Default = new EnumerableEqualityComparer();

        public bool Equals(IEnumerable x, IEnumerable y)
        {
            if (x is null || y is null)
                return (x == y) ? true : false;

            if (!x.GetEnumerator().MoveNext() && !y.GetEnumerator().MoveNext())
            {
                // two empty enumerables of same type are cionsidered of same value
                return true;
            }

            return false;
        }

        public int GetHashCode(IEnumerable obj)
        {
            throw new NotImplementedException();
        }
    }
}