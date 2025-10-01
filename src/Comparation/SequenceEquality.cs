#if !NETSTANDARD2_0
using System;
#endif
using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class SequenceEquality<T> : IEqualityComparer<IEnumerable<T?>>
    {
        private readonly IEqualityComparer<T> itemEquality;

        public SequenceEquality(IEqualityComparer<T> itemEquality)
        {
            this.itemEquality = itemEquality;
        }

        public bool Equals(IEnumerable<T?>? x, IEnumerable<T?>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            return x!.SequenceEqual(y!, itemEquality);
        }

        public int GetHashCode(IEnumerable<T?> obj)
        {
            var hash = new HashCode();
            foreach (var item in obj)
            {
                var itemHash = item is { } value
                    ? itemEquality.GetHashCode(value)
                    : 0;

                hash.Add(itemHash);
            }

            return hash.ToHashCode();
        }
    }
}