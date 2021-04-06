using System;
using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Order.Aspects
{
    public sealed class OrderShouldBeEqual<T> : Test
    {
        private readonly IComparer<T> sutA;
        private readonly IComparer<T> sutB;
        private readonly T a;
        private readonly T b;

        public OrderShouldBeEqual(IComparer<T> sutA, IComparer<T> sutB, T a, T b)
        {
            this.sutA = sutA;
            this.sutB = sutB;
            this.a = a;
            this.b = b;
        }

        public override void Run()
        {
            var resultA = Math.Sign(sutA.Compare(a, b));
            var resultB = Math.Sign(sutB.Compare(a, b));

            resultA.Should().Be(resultB);
        }
    }
}