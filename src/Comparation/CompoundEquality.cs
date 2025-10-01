#if !NETSTANDARD2_0
using System;
#endif
using System.Collections.Generic;

namespace Comparation
{
    public sealed class CompoundEquality<TSubject> : IEqualityComparer<TSubject>
    {
        private readonly IEqualityComparer<TSubject> aspectA;
        private readonly IEqualityComparer<TSubject> aspectB;

        public CompoundEquality(
            IEqualityComparer<TSubject> aspectA,
            IEqualityComparer<TSubject> aspectB)
        {
            this.aspectA = aspectA;
            this.aspectB = aspectB;
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

            return aspectA.Equals(x, y) && aspectB.Equals(x, y);
        }

        public int GetHashCode(TSubject obj)
        {
            if (obj is null)
            {
                return 0;
            }

            var hash = new HashCode();

            var aspectAHash = aspectA.GetHashCode(obj);
            hash.Add(aspectAHash);

            var aspectBHash = aspectB.GetHashCode(obj);
            hash.Add(aspectBHash);

            return hash.ToHashCode();
        }
    }
}