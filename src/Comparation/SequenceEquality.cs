using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

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

            return (GuessCount(x), GuessCount(y)) switch
            {
                (null, _) => Fallback(),
                (_, null) => Fallback(),
                ({ } xCount, { } yCount) => xCount == yCount && Fallback()
            };

            bool Fallback()
            {
                using var xEnumerator = x.GetEnumerator();
                using var yEnumerator = y.GetEnumerator();
                while (true)
                {
                    switch (xEnumerator.MoveNext(), yEnumerator.MoveNext())
                    {
                        case (false, false):
                            return true;
                        case (true, true):
                            if (!itemEquality.Equals(xEnumerator.Current!, yEnumerator.Current!))
                            {
                                return false;
                            }

                            continue;
                        default:
                            return false;
                    }
                }
            }
        }

        public int GetHashCode(IEnumerable<T?> obj) => obj.Aggregate(
            0,
            (hashCode, item) =>
            {
                var itemHashCode = item is { } value
                    ? itemEquality.GetHashCode(value)
                    : 0;

                return unchecked((hashCode * 397) ^ itemHashCode);
            });

        private static int? GuessCount([NoEnumeration] IEnumerable<T?> x) => x switch
        {
            IReadOnlyCollection<T> collection => collection.Count,
            _ => null
        };
    }
}