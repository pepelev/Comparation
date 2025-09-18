using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class CompositeEquality<TSubject> : IEqualityComparer<TSubject>
    {
        private readonly IReadOnlyCollection<IEqualityComparer<TSubject>> aspects;

        public CompositeEquality(params IEqualityComparer<TSubject>[] aspects)
            : this(aspects as IReadOnlyCollection<IEqualityComparer<TSubject>>)
        {
        }

        public CompositeEquality(IReadOnlyCollection<IEqualityComparer<TSubject>> aspects)
        {
            this.aspects = aspects;
        }

        public bool Equals(TSubject? x, TSubject? y)
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

            return aspects.All(aspect => aspect.Equals(x, y));
        }

        public int GetHashCode(TSubject obj)
        {
            if (obj is null)
            {
                return 0;
            }

            return aspects.Aggregate(
                0,
                (hashCode, aspect) => unchecked(
                    (hashCode * 397) ^ aspect.GetHashCode(obj)
                )
            );
        }
    }
}