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
        public IEnumerator<TestCaseData> GetEnumerator() => Sequence.Concat(
                new WorkAsExpected(),
                new ExampleTests(),
                new Commutative(),
                new TreatNullsAsEqual(),
                new GiveSameHashCodeForEqualObjects()
            )
            .Select(@case => new NamePrefixing<Test>("Equality", @case))
            .Select(@case => new TestCaseData(@case.Value).SetName(@case.Name).SetCategory("Equality"))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}