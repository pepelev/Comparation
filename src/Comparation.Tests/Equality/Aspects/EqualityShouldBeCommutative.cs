using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldBeCommutative<T> : Test
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T a;
        private readonly T b;

        public EqualityShouldBeCommutative(IEqualityComparer<T> equality, T a, T b)
        {
            this.equality = equality;
            this.a = a;
            this.b = b;
        }

        public override void Run()
        {
            var ab = equality.Equals(a, b);
            var ba = equality.Equals(b, a);

            new[] {ab, ba}.Distinct().Should().HaveCount(1);
        }
    }
}