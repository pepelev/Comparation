using System.Collections.Generic;

namespace Comparation
{
    public sealed class ReferenceEquality<T> : IEqualityComparer<T> where T : class
    {
        internal static ReferenceEquality<T> Singleton { get; } = new();

        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj) => obj.GetHashCode();
    }
}