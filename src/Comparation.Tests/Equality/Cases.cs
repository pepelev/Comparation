using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Tests;
using NUnit.Framework;

namespace Comparation.Tests.Equality
{
    public sealed class Cases : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator() => new ExampleTests()
            .Select(@case => new NamePrefixing("Equality", @case))
            .Select(@case => new TestCaseData(@case).SetName(@case.Name))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}