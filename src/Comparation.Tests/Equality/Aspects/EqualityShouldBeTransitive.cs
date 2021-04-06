using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldBeTransitive<T> : Test
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T? a;
        private readonly T? b;
        private readonly T? c;

        public EqualityShouldBeTransitive(IEqualityComparer<T> equality, T? a, T? b, T? c)
        {
            this.equality = equality;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override void Run()
        {
            var ab = equality.Equals(a, b);
            var ac = equality.Equals(a, c);
            var bc = equality.Equals(b, c);

            if (ab && bc)
            {
                ac.Should().BeTrue();
            }
        }
    }
}