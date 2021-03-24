using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldWorkAsExpected<T> : Test
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T? a;
        private readonly T? b;
        private readonly bool expectation;

        public EqualityShouldWorkAsExpected(IEqualityComparer<T> equality, T? a, T? b, bool expectation)
        {
            this.equality = equality;
            this.a = a;
            this.b = b;
            this.expectation = expectation;
        }

        public override void Run()
        {
            equality.Equals(a, b).Should().Be(expectation);
        }
    }
}