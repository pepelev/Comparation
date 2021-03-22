using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class CollectionEquality<T> : IEqualityComparer<IReadOnlyCollection<T>>
    {
        private readonly IEqualityComparer<T> itemEquality;

        public CollectionEquality(IEqualityComparer<T> itemEquality)
        {
            this.itemEquality = itemEquality;
        }

        public bool Equals(IReadOnlyCollection<T> x, IReadOnlyCollection<T> y)
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

        public int GetHashCode(IReadOnlyCollection<T> obj)
        {
            var hashCodes = obj
                .Select(itemEquality.GetHashCode)
                .OrderBy(hashCode => hashCode)
                .ToList();

            return hashCodes.Aggregate(
                0,
                (currentHashCode, hashCode) => unchecked(
                    (currentHashCode * 397) ^ hashCode
                )
            );
        }

        private Dictionary<T, int> IndexItemCounts(IEnumerable<T> x)
        {
            var counts = new Dictionary<T, int>(itemEquality);
            foreach (var item in x)
            {
                var newCount = counts.TryGetValue(item, out var count)
                    ? count + 1
                    : 1;
                counts[item] = newCount;
            }

            return counts;
        }

        private static bool Equals(IEnumerable<T> y, Dictionary<T, int> counts)
        {
            foreach (var item in y)
            {
                if (counts.TryGetValue(item, out var count))
                {
                    if (count == 1)
                    {
                        counts.Remove(item);
                    }
                    else
                    {
                        counts[item] = count - 1;
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