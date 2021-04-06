using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Order.Aspects
{
    public sealed class OrderShouldBeTransitive<T> : Test
    {
        private readonly IComparer<T> sut;
        private readonly T a;
        private readonly T b;
        private readonly T c;

        public OrderShouldBeTransitive(IComparer<T> sut, T a, T b, T c)
        {
            this.sut = sut;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override void Run()
        {
            if (sut.Compare(a, b) > 0 && sut.Compare(b, c) > 0)
            {
                sut.Compare(a, c).Should().BePositive();
            }
        }
    }
}