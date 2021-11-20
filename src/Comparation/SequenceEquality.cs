using System.Collections.Generic;
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
                ({ } xCount, { } yCount) => xCount == yCount && Fallback(),
                _ => Fallback()
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

        public int GetHashCode(IEnumerable<T?> obj)
        {
            var hashCode = 0;
            foreach (var item in obj)
            {
                var itemHashCode = item is { } value
                    ? itemEquality.GetHashCode(value)
                    : 0;

                hashCode = unchecked((hashCode * 397) ^ itemHashCode);
            }

            return hashCode;
        }

        private static int? GuessCount([NoEnumeration] IEnumerable<T?> x) => x switch
        {
            IReadOnlyCollection<T> collection => collection.Count,
            _ => null
        };
    }
}