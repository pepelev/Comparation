using System;
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
        private readonly int expectation;

        public OrderShouldWorkAsExpected(IComparer<T> sut, T a, T b, int expectation)
        {
            this.sut = sut;
            this.a = a;
            this.b = b;
            this.expectation = expectation;
        }

        public override void Run()
        {
            Math.Sign(sut.Compare(a, b)).Should().Be(expectation);
        }
    }
}