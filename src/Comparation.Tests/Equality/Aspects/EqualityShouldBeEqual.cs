using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldBeEqual<T> : Test
    {
        private readonly IEqualityComparer<T> equalityA;
        private readonly IEqualityComparer<T> equalityB;
        private readonly T? a;
        private readonly T? b;

        public EqualityShouldBeEqual(IEqualityComparer<T> equalityA, IEqualityComparer<T> equalityB, T? a, T? b)
        {
            this.equalityA = equalityA;
            this.equalityB = equalityB;
            this.a = a;
            this.b = b;
        }

        public override void Run()
        {
            var resultA = equalityA.Equals(a, b);
            var resultB = equalityB.Equals(a, b);

            new[] {resultA, resultB}.Distinct().Should().HaveCount(1);
        }
    }
}