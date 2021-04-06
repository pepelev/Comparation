using System;
using System.Collections.Generic;
using Comparation.Tests.Core;
using FluentAssertions;

namespace Comparation.Tests.Order.Aspects
{
    public sealed class OrderShouldReverseResultOnArgumentFlip<T> : Test
    {
        private readonly IComparer<T> sut;
        private readonly T a;
        private readonly T b;

        public OrderShouldReverseResultOnArgumentFlip(IComparer<T> sut, T a, T b)
        {
            this.sut = sut;
            this.a = a;
            this.b = b;
        }

        public override void Run()
        {
            var directSign = Math.Sign(sut.Compare(a, b));
            var reversedSign = Math.Sign(sut.Compare(b, a));
            directSign.Should().Be(-reversedSign);
        }
    }
}