using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class SequenceEquality<T> : IEqualityComparer<IEnumerable<T>>
    {
        private readonly IEqualityComparer<T> itemEquality;

        public SequenceEquality(IEqualityComparer<T> itemEquality)
        {
            this.itemEquality = itemEquality;
        }

        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
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

            return (GuessCount(x), GuessCount(y)) switch
            {
                (null, _) => Fallback(),
                (_, null) => Fallback(),
                ({ } xCount, { } yCount) => xCount == yCount && Fallback()
            };

            bool Fallback() => x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T> obj) => obj.Aggregate(
            0,
            (hashCode, item) => unchecked(
                (hashCode * 397) ^ itemEquality.GetHashCode(item)
            )
        );

        private static int? GuessCount(IEnumerable<T> x) => x switch
        {
            IReadOnlyCollection<T> collection => collection.Count,
            _ => null
        };
    }
}