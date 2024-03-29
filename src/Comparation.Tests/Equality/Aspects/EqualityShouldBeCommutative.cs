﻿using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Equality.Aspects
{
    public sealed class EqualityShouldBeCommutative<T> : Test
    {
        private readonly T a;
        private readonly T b;
        private readonly IEqualityComparer<T> equality;

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

            ab.Should().Be(ba);
        }
    }
}