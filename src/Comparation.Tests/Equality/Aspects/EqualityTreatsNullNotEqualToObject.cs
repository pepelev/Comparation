using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityTreatsNullNotEqualToObject<T> : Test where T : class
    {
        private readonly IEqualityComparer<T> equality;
        private readonly T a;

        public EqualityTreatsNullNotEqualToObject(IEqualityComparer<T> equality, T a)
        {
            this.equality = equality;
            this.a = a;
        }

        public override void Run()
        {
            equality.Equals(a, null).Should().BeFalse();
        }
    }
}