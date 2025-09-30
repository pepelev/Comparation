using System.Collections.Generic;

namespace Comparation
{
    public sealed class NullableEquality<TSubject> : IEqualityComparer<TSubject?> where TSubject : struct
    {
        private readonly IEqualityComparer<TSubject> equality;

        public NullableEquality(IEqualityComparer<TSubject> equality)
        {
            this.equality = equality;
        }

        public bool Equals(TSubject? x, TSubject? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }
            
            return equality.Equals(x.Value, y.Value);
        }

        public int GetHashCode(TSubject? obj)
        {
            return obj == null
                ? 0
                : equality.GetHashCode(obj.Value);
        }
    }
}