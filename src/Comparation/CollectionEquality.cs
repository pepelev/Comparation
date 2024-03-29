﻿using System;
using System.Collections.Generic;

namespace Comparation
{
    public sealed class CollectionEquality<T> : IEqualityComparer<IReadOnlyCollection<T?>>
    {
        private readonly Box<T>.Equality itemEquality;

        public CollectionEquality(IEqualityComparer<T> itemEquality)
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

            if (x.Count != y.Count)
            {
                return false;
            }

            var counts = IndexItemCounts(x);
            return Equals(y, counts);
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
                hashCodes[i++] = itemEquality.GetHashCode(new Box<T?>(item));
            }

#if NETCOREAPP2_1_OR_GREATER
            hashCodes.Sort();
#else
            Array.Sort(hashCodes);
#endif

            var resultHashCode = 0;
            foreach (var hashCode in hashCodes)
            {
                resultHashCode = unchecked(
                    (resultHashCode * 397) ^ hashCode
                );
            }

            return resultHashCode;
        }

        private Dictionary<Box<T?>, int> IndexItemCounts(IEnumerable<T?> x)
        {
            var counts = new Dictionary<Box<T?>, int>(itemEquality);
            foreach (var item in x)
            {
                var box = new Box<T?>(item);
                var newCount = counts.TryGetValue(box, out var count)
                    ? count + 1
                    : 1;
                counts[box] = newCount;
            }

            return counts;
        }

        private static bool Equals(IEnumerable<T?> y, Dictionary<Box<T?>, int> counts)
        {
            foreach (var item in y)
            {
                var box = new Box<T?>(item);
                if (counts.TryGetValue(box, out var count))
                {
                    if (count == 1)
                    {
                        counts.Remove(box);
                    }
                    else
                    {
                        counts[box] = count - 1;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}