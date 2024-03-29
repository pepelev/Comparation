﻿using System.Collections;
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

        public IEnumerator<TestCaseData> GetEnumerator() => Sequence.Concat(
                new WorkAsExpected(),
                new Examples(),
                new Commutative(),
                new TreatNullsAsEqual(),
                new GiveSameHashCodeForEqualObjects(),
                new TreatTreatsNullNotEqualToObject(),
                new Transitive(),
                new Equal()
            )
            .Select(@case => new NamePrefixing<Test>(Category, @case))
            .Select(@case => new TestCaseData(@case.Value).SetName(@case.Name).SetCategory(Category))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}