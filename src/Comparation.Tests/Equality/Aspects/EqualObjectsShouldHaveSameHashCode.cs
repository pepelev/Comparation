using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualObjectsShouldHaveSameHashCode<T> : Test
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T a;
        private readonly T b;

        public EqualObjectsShouldHaveSameHashCode(IEqualityComparer<T> equality, T a, T b)
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