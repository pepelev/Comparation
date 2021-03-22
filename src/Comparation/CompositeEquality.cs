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

        public bool Equals(Subject x, Subject y) => aspects.All(aspect => aspect.Equals(x, y));

        public int GetHashCode(Subject obj) => aspects.Aggregate(
            0,
            (hashCode, aspect) => unchecked(
                (hashCode * 397) ^ aspect.GetHashCode(obj)
            )
        );
    }
}