#if !NETSTANDARD2_0
using System;
#endif
using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public sealed class CompositeEquality<TSubject> : IEqualityComparer<TSubject>
    {
        private readonly IEqualityComparer<TSubject>[] aspects;

        public CompositeEquality(params IEqualityComparer<TSubject>[] aspects)
        {
            this.aspects = aspects;
        }

        public CompositeEquality(IReadOnlyCollection<IEqualityComparer<TSubject>> aspects)
            : this(aspects.ToArray())
        {
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

            foreach (var aspect in aspects)
            {
                if (!aspect.Equals(x, y))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(TSubject obj)
        {
            if (obj is null)
            {
                return 0;
            }

            var hash = new HashCode();
            foreach (var aspect in aspects)
            {
                var aspectHash = aspect.GetHashCode(obj);
                hash.Add(aspectHash);
            }

            return hash.ToHashCode();
        }
    }
}