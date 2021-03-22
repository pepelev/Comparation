using System.Collections.Generic;

namespace Comparation
{
    public sealed class OptimisticReferenceEquality<T> : IEqualityComparer<T> where T : class
    {
        private readonly IEqualityComparer<T> equality;

        public OptimisticReferenceEquality(IEqualityComparer<T> equality)
        {
            this.equality = equality;
        }

        public bool Equals(T x, T y)
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

            return equality.Equals(x, y);
        }

        public int GetHashCode(T obj) => equality.GetHashCode(obj);
    }
}