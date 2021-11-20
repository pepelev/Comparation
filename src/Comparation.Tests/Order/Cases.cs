using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Order.Tests;
using NUnit.Framework;

namespace Comparation.Tests.Order
{
    public sealed class Cases : IEnumerable<TestCaseData>
    {
        private const string Category = "Order";

        public IEnumerator<TestCaseData> GetEnumerator() => Sequence.Concat(
                new WorkAsExpected()
            )
            .Select(@case => new NamePrefixing<Test>(Category, @case))
            .Select(@case => new TestCaseData(@case.Value).SetName(@case.Name).SetCategory(Category))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}