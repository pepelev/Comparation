using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldTreatsNullsAsEqual<T> : Test where T : class
    {
        private readonly IEqualityComparer<T> equality;

        public EqualityShouldTreatsNullsAsEqual(IEqualityComparer<T> equality)
        {
            this.equality = equality;
        }

        public override void Run()
        {
            equality.Equals(null, null).Should().BeTrue();
        }
    }
}