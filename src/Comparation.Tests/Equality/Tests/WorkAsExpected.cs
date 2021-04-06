using System;
using System.Collections.Generic;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;
using static Comparation.Equality;
using static Comparation.Tests.Core.Sequence;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class WorkAsExpected : Suite
    {
        private static IEqualityComparer<string> StringByLength => Of<string>().By(@string => @string.Length);
        private static IEqualityComparer<char> CharCaseInsensitively => Of<char>().By(char.ToLowerInvariant);

        private static IEnumerable<Named<Test>> By() => new[]
        {
            Test("positive by", StringByLength, "Hello", "world", true),
            Test("negative by", StringByLength, "Quick", "fox", false)
        };

        private static IEnumerable<Named<Test>> ByReference()
        {
            var sut = ByReference<string>();
            return new[]
            {
                Test("positive by reference (interning)", sut, "Hello", "Hello", true),
                Test("negative by reference", sut, "Quick", string.Concat("Qui", "ck"), false)
            };
        }

        private static IEnumerable<Named<Test>> ByWithEquality()
        {
            var sut = Of<string>().By(
                @string => @string[0],
                CharCaseInsensitively
            );
            return new[]
            {
                Test("positive single by with equality", sut, "Firmware", "fox", true),
                Test("negative single by with equality", sut, "Yeti", "Bigfoot", false)
            };
        }

        private static IEnumerable<Named<Test>> AndBy()
        {
            var sut = StringByLength.AndBy(@string => @string[0]);
            return new[]
            {
                Test("positive and-by", sut, "Brown", "Beast", true),
                Test("negative and-by on first field", sut, "Apple", "Alpine", false),
                Test("negative and-by on second field", sut, "Fear", "Calm", false)
            };
        }

        private static IEnumerable<Named<Test>> AndByWithEquality()
        {
            var sut = StringByLength.AndBy(@string => @string[0], CharCaseInsensitively);
            return new[]
            {
                Test("positive and-by with equality", sut, "Brown", "blast", true),
                Test("negative and-by with equality on first field", sut, "Apple", "Alpine", false),
                Test("negative and-by with equality on second field", sut, "Fear", "Calm", false)
            };
        }

        private static IEnumerable<Named<Test>> Composite()
        {
            var sut = Of<string>().Composite(
                StringByLength,
                Of<string>().By(
                    @string => @string[0],
                    CharCaseInsensitively
                )
            );
            return new[]
            {
                Test("positive composite", sut, "Brown", "blast", true),
                Test("negative composite on first", sut, "Apple", "Alpine", false),
                Test("negative composite on second", sut, "Fear", "Calm", false)
            };
        }

        private static IEnumerable<Named<Test>> Sequence()
        {
            var sut = Of<string>().Sequence();
            return new[]
            {
                Test("positive sequence (empty)", sut, Array.Empty<string>(), Array.Empty<string>(), true),
                Test("positive sequence (single element)", sut, new[] {"Apple"}, new[] {"Apple"}, true),
                Test("positive sequence (single null element)", sut, new string?[] {null}, new string?[] {null}, true),
                Test("positive sequence (several elements)", sut, new[] {"Apple", "Dog"}, new[] {"Apple", "Dog"}, true),
                Test("positive sequence (several elements with nulls)", sut, new[] {"Apple", null}, new[] {"Apple", null}, true),
                Test("negative sequence (empty vs non-empty)", sut, Array.Empty<string>(), new[] {"Dog"}, false),
                Test("negative sequence (different elements)", sut, new[] {"Dog"}, new[] {"Cat"}, false),
                Test("negative sequence (different length)", sut, new[] {"Dog", "Cat"}, new[] {"Cat"}, false)
            };
        }

        private static IEnumerable<Named<Test>> SequenceWithEquality()
        {
            var sut = Of<string>().Sequence(
                Of<string>().By(@string => @string[0])
            );
            return new[]
            {
                Test("positive sequence with equality (empty)", sut, Array.Empty<string>(), Array.Empty<string>(), true),
                Test("positive sequence with equality (single element)", sut, new[] {"Alpine"}, new[] {"Apple"}, true),
                Test("positive sequence with equality (single null element)", sut, new string?[] {null}, new string?[] {null}, true),
                Test("positive sequence with equality (several elements)", sut, new[] {"Can", "Dog"}, new[] {"Cat", "Dog"}, true),
                Test("positive sequence with equality (several elements with nulls)", sut, new[] {"Can", null}, new[] {"Cat", null}, true),
                Test("negative sequence with equality (empty vs non-empty)", sut, Array.Empty<string>(), new[] {"Dog"}, false),
                Test("negative sequence with equality (different elements)", sut, new[] {"Dog"}, new[] {"Cat"}, false),
                Test("negative sequence with equality (different length)", sut, new[] {"Dog", "Cat"}, new[] {"Cat"}, false)
            };
        }

        private static IEnumerable<Named<Test>> Collection()
        {
            var sut = Of<string>().Collection();
            return new[]
            {
                Test("positive collection (empty)", sut, Array.Empty<string>(), Array.Empty<string>(), true),
                Test("positive collection (single element)", sut, new[] {"Apple"}, new[] {"Apple"}, true),
                Test("positive collection (single null element)", sut, new string?[] {null}, new string?[] {null}, true),
                Test("positive collection (several elements)", sut, new[] {"Apple", "Dog"}, new[] {"Apple", "Dog"}, true),
                Test("positive collection (several elements out of order)", sut, new[] {"Dog", "Apple"}, new[] {"Apple", "Dog"}, true),
                Test("positive collection (several elements out of order with nulls)", sut, new[] {null, "Dog"}, new[] {"Dog", null}, true),
                Test("positive collection (several elements out of order with duplicates)", sut, new[] {"Dog", "Apple", "Dog"}, new[] {"Dog", "Dog", "Apple"}, true),
                Test("negative collection (empty vs non-empty)", sut, Array.Empty<string>(), new[] {"Dog"}, false),
                Test("negative collection (different elements)", sut, new[] {"Dog"}, new[] {"Cat"}, false),
                Test("negative collection (different length)", sut, new[] {"Dog", "Cat"}, new[] {"Cat"}, false),
                Test("negative collection (different duplicates)", sut, new[] {"Dog", "Dog", "Cat"}, new[] { "Cat", "Cat", "Dog" }, false)
            };
        }

        private static IEnumerable<Named<Test>> CollectionWithEquality()
        {
            var sut = Of<string>().Collection(
                Of<string>().By(@string => @string[0])
            );
            return new[]
            {
                Test("positive collection with equality (empty)", sut, Array.Empty<string>(), Array.Empty<string>(), true),
                Test("positive collection with equality (single element)", sut, new[] {"Alpine"}, new[] {"Apple"}, true),
                Test("positive collection with equality (single null element)", sut, new string?[] {null}, new string?[] {null}, true),
                Test("positive collection with equality (several elements)", sut, new[] {"Apple", "Digger"}, new[] {"Apple", "Dog"}, true),
                Test("positive collection with equality (several elements out of order)", sut, new[] {"Dog", "Apple"}, new[] {"Arena", "Disk"}, true),
                Test("positive collection with equality (several elements out of order with nulls)", sut, new[] {"Dog", null}, new[] {null, "Disk"}, true),
                Test("positive collection with equality (several elements out of order with duplicates)", sut, new[] {"Dog", "Apple", "Disk"}, new[] {"Dog", "Door", "Author"}, true),
                Test("negative collection with equality (empty vs non-empty)", sut, Array.Empty<string>(), new[] {"Dog"}, false),
                Test("negative collection with equality (different elements)", sut, new[] {"Dog"}, new[] {"Cat"}, false),
                Test("negative collection with equality (different length)", sut, new[] {"Dog", "Cat"}, new[] {"Cat"}, false),
                Test("negative collection with equality (different duplicates)", sut, new[] {"Dog", "Dog", "Cat"}, new[] { "Cat", "Cat", "Dog" }, false)
            };
        }

        public override IEnumerator<Named<Test>> GetEnumerator()
        {
            var cases = Concat(
                By(),
                ByReference(),
                ByWithEquality(),
                AndBy(),
                AndByWithEquality(),
                Composite(),
                Sequence(),
                SequenceWithEquality(),
                Collection(),
                CollectionWithEquality()
            );

            return cases.GetEnumerator();
        }

        private static Named<Test> Test<T>(string name, IEqualityComparer<T> sut, T? a, T? b, bool expectation)
        {
            var test = new EqualityShouldWorkAsExpected<T>(sut, a, b, expectation);
            return new NamedByType<Test>(name, test);
        }
    }
}