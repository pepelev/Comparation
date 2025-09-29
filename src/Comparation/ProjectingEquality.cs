using System;
using System.Collections.Generic;

namespace Comparation
{
    public sealed class ProjectingEquality<TSubject, TProjection> : IEqualityComparer<TSubject>
    {
        private readonly Func<TSubject, TProjection?> projection;
        private readonly IEqualityComparer<TProjection> equality;

        public ProjectingEquality(Func<TSubject, TProjection?> projection, IEqualityComparer<TProjection> equality)
        {
            this.projection = projection;
            this.equality = equality;
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

#if NETSTANDARD2_0 || NETSTANDARD2_1
            return equality.Equals(
                projection(x)!,
                projection(y)!
            );
#else
            return equality.Equals(
                projection(x),
                projection(y)
            );
#endif
        }

        public int GetHashCode(TSubject obj) => projection(obj) is { } value
            ? equality.GetHashCode(value)
            : 0;
    }
}