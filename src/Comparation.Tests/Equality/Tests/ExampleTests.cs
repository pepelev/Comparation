using System.Collections;
using System.Collections.Generic;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class ExampleTests : IEnumerable<NamedTest>
    {
        public IEnumerator<NamedTest> GetEnumerator()
        {
            yield return new NamedByType(
                "positive single by",
                new EqualityShouldWorkAsExpected<string>(
                    Comparation.Equality.Of<string>().By(@string => @string.Length),
                    "Hello",
                    "world",
                    true
                )
            );

            yield return new NamedByType(
                "positive",
                new EqualObjectsShouldHaveSameHashCode<string>(
                    Comparation.Equality.Of<string>().By(@string => @string.Length),
                    "cat",
                    "Dog"
                )
            );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}