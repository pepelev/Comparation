using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldGiveSameHashCodeForEqualObjects<T> : Test
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T a;
        private readonly T b;

        public EqualityShouldGiveSameHashCodeForEqualObjects(IEqualityComparer<T> equality, T a, T b)
        {
            this.equality = equality;
            this.a = a;
            this.b = b;
        }

        public override void Run()
        {
            if (a is not null && b is not null && equality.Equals(a, b))
            {
                var aHashCode = equality.GetHashCode(a);
                var bHashCode = equality.GetHashCode(b);

                aHashCode.Should().Be(bHashCode);
            }
        }
    }
}