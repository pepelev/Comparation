using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class CollectionEquality<T> : IEqualityComparer<IReadOnlyCollection<T?>>
    {
        private readonly Value<T>.Equality itemEquality;

        public CollectionEquality(IEqualityComparer<T> itemEquality)
        {
            this.itemEquality = new Value<T>.Equality(itemEquality);
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
            var hashCodes = collection
                .Select(item => itemEquality.GetHashCode(new Value<T?>(item)))
                .OrderBy(hashCode => hashCode)
                .ToList();

            return hashCodes.Aggregate(
                0,
                (currentHashCode, hashCode) => unchecked(
                    (currentHashCode * 397) ^ hashCode
                )
            );
        }

        private Dictionary<Value<T?>, int> IndexItemCounts(IEnumerable<T?> x)
        {
            var counts = new Dictionary<Value<T?>, int>(itemEquality);
            foreach (var item in x)
            {
                var value = new Value<T?>(item);
                var newCount = counts.TryGetValue(value, out var count)
                    ? count + 1
                    : 1;
                counts[value] = newCount;
            }

            return counts;
        }

        private static bool Equals(IEnumerable<T?> y, Dictionary<Value<T?>, int> counts)
        {
            foreach (var item in y)
            {
                var value = new Value<T?>(item);
                if (counts.TryGetValue(value, out var count))
                {
                    if (count == 1)
                    {
                        counts.Remove(value);
                    }
                    else
                    {
                        counts[value] = count - 1;
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