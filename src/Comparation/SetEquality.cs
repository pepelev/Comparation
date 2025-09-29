using System;
using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class SetEquality<T> : IEqualityComparer<IReadOnlyCollection<T?>>
    {
        private readonly Box<T>.Equality itemEquality;

        public SetEquality(IEqualityComparer<T> itemEquality)
        {
            this.itemEquality = new Box<T>.Equality(itemEquality);
        }

        public bool Equals(IReadOnlyCollection<T?>? x, IReadOnlyCollection<T?>? y)
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

            var xEmpty = x.Count == 0;
            var yEmpty = y.Count == 0;
            if (xEmpty != yEmpty)
            {
                return false;
            }

            var setX = new HashSet<Box<T?>>(
                x.Select(Box.Wrap),
                itemEquality
            );

            var itemsY = y.Select(Box.Wrap);
            return setX.SetEquals(itemsY);
        }

        public int GetHashCode(IReadOnlyCollection<T?> collection)
        {
            var count = collection.Count;
#if NETCOREAPP2_1_OR_GREATER
            var hashCodes = count <= 1024 / sizeof(int)
                ? stackalloc int[count]
                : new int[count];
#else
            var hashCodes = new int[count];
#endif
            var i = 0;
            foreach (var item in collection)
            {
                hashCodes[i++] = itemEquality.GetHashCode(Box.Wrap(item));
            }

#if NETCOREAPP2_1_OR_GREATER
            hashCodes.Sort();
#else
            Array.Sort(hashCodes);
#endif

            var result = new HashCode();
            var lastHashCode = 0;
            foreach (var hashCode in hashCodes)
            {
                if (lastHashCode == hashCode)
                {
                    continue;
                }

                result.Add(hashCode);
                lastHashCode = hashCode;
            }

            return result.ToHashCode();
        }
    }
}