using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Order.Aspects
{
    public sealed class OrderShouldWorkAsExpected<T> : Test
    {
        private readonly IComparer<T> sut;
        private readonly T a;
        private readonly T b;
        private readonly Comparation.Order.Sign expectation;

        public OrderShouldWorkAsExpected(IComparer<T> sut, T a, T b, Comparation.Order.Sign expectation)
        {
            this.sut = sut;
            this.a = a;
            this.b = b;
            this.expectation = expectation;
        }

        public override void Run()
        {
            sut.Sign(a, b).Should().Be(expectation);
        }
    }
}