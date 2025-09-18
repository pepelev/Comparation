using System;
using System.Collections.Generic;

namespace Comparation
{
    // todo make interface generic parameter nullable
    public sealed class ProjectingEquality<TSubject, TProjection> : IEqualityComparer<TSubject>
    {
        private readonly Func<TSubject, TProjection> projection;
        private readonly IEqualityComparer<TProjection> equality;

        public ProjectingEquality(Func<TSubject, TProjection> projection, IEqualityComparer<TProjection> equality)
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

            return equality.Equals(
                projection(x),
                projection(y)
            );
        }

        public int GetHashCode(TSubject obj) => projection(obj) is { } value
            ? equality.GetHashCode(value)
            : 0;
    }
}