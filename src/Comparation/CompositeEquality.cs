using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class CompositeEquality<Subject> : IEqualityComparer<Subject>
    {
        private readonly IReadOnlyCollection<IEqualityComparer<Subject>> aspects;

        public CompositeEquality(params IEqualityComparer<Subject>[] aspects)
            : this(aspects as IReadOnlyCollection<IEqualityComparer<Subject>>)
        {
        }

        public CompositeEquality(IReadOnlyCollection<IEqualityComparer<Subject>> aspects)
        {
            this.aspects = aspects;
        }

        public bool Equals(Subject? x, Subject? y)
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

        public int GetHashCode(Subject obj)
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