using System;
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
        private const string Category = "Equality";

        private static EqualitySuite<IReadOnlyCollection<string?>> StringSetSuite()
        {
            // ReSharper disable InconsistentNaming
            Named<IReadOnlyCollection<string?>?> nullSet = (nameof(nullSet), null);
            Named<IReadOnlyCollection<string?>?> emptySet = (nameof(emptySet), Array.Empty<string>());
            Named<IReadOnlyCollection<string?>?> singleNull = (nameof(singleNull), new string?[] { null });
            Named<IReadOnlyCollection<string?>?> singleA = (nameof(singleA), new[] { "a" });
            Named<IReadOnlyCollection<string?>?> singleBigA = (nameof(singleBigA), new[] { "A" });
            Named<IReadOnlyCollection<string?>?> twoAs = (nameof(twoAs), new[] { "a", "A" });
            Named<IReadOnlyCollection<string?>?> ab = (nameof(ab), new[] { "a", "b" });
            Named<IReadOnlyCollection<string?>?> ba = (nameof(ba), new[] { "b", "a" });
            Named<IReadOnlyCollection<string?>?> abc = (nameof(abc), new[] { "a", "b", "c" });
            Named<IReadOnlyCollection<string?>?> ab2c = (nameof(ab2c), new[] { "a", "b", "c", "b" });
            Named<IReadOnlyCollection<string?>?> abnull = (nameof(abnull), new[] { "a", "b", null });
            Named<IReadOnlyCollection<string?>?> nullba = (nameof(nullba), new[] { null, "b", "a" });
            // ReSharper restore InconsistentNaming

            return new EqualitySuite<IReadOnlyCollection<string?>>(
                nameof(SetEquality<string>),
                new SetEquality<string>(
                    Comparation.Equality.Of<string>().By(@string => @string.ToLowerInvariant())
                ),
                new[]
                {
                    new[] { nullSet },
                    new[] { emptySet },
                    new[] { singleNull },
                    new[] { singleA, singleBigA, twoAs },
                    new[] { ab, ba },
                    new[] { abc, ab2c },
                    new[] { abnull, nullba }
                }
            );
        }

        public IEnumerator<TestCaseData> GetEnumerator() => Sequence.Concat(
                new WorkAsExpected(),
                new Examples(),
                new Commutative(),
                new TreatNullsAsEqual(),
                new GiveSameHashCodeForEqualObjects(),
                new TreatTreatsNullNotEqualToObject(),
                new Transitive(),
                new Equal(),
                StringSetSuite()
            )
            .Select(@case => new NamePrefixing<Test>(Category, @case))
            .Select(@case => new TestCaseData(@case.Value).SetName(@case.Name).SetCategory(Category))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}