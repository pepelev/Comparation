using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Equality.Aspects;

namespace Comparation.Tests.Core
{
    public sealed class EqualitySuite<T> : IEnumerable<Named<Test>>
    {
        private readonly string name;
        private readonly IEqualityComparer<T> sut;
        private readonly Named<T?>[][] equalGroups;

        public EqualitySuite(string name, IEqualityComparer<T> sut, Named<T?>[][] equalGroups)
        {
            this.name = name;
            this.sut = sut;
            this.equalGroups = equalGroups;
        }

        private IEnumerable<Named<Test>> Yield()
        {
            var allItems = equalGroups.SelectMany(group => group).ToList();
            var uniqueNames = allItems.Select(item => item.Name).Distinct().ToList();
            if (uniqueNames.Count != allItems.Count)
            {
                throw new Exception("Duplicate items");
            }

            return Cases();

            IEnumerable<Named<Test>> Cases()
            {
                for (var i = 0; i < equalGroups.Length; i++)
                {
                    for (var j = 0; j < equalGroups.Length; j++)
                    {
                        for (var k = 0; k < equalGroups[i].Length; k++)
                        {
                            for (var m = 0; m < equalGroups[j].Length; m++)
                            {
                                var a = equalGroups[i][k];
                                var b = equalGroups[j][m];
                                var expectation = i == j;
                                var sign = expectation
                                    ? "=="
                                    : "!=";
                                yield return (
                                    $"{name}: {a.Name} {sign} {b.Name}",
                                    new EqualityShouldWorkAsExpected<T>(
                                        sut,
                                        a.Value,
                                        b.Value,
                                        expectation: expectation
                                    )
                                );

                                if (expectation)
                                {
                                    yield return (
                                        $"{name}: HashCode({a.Name}) {sign} HashCode({b.Name})",
                                        new EqualityShouldGiveSameHashCodeForEqualObjects<T>(
                                            sut,
                                            a.Value,
                                            b.Value
                                        )
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }

        public IEnumerator<Named<Test>> GetEnumerator() => Yield().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}