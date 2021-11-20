using System.Collections.Generic;
using Comparation.Tests.Core;
using Comparation.Tests.Order.Aspects;
using static Comparation.Order;
using static Comparation.Tests.Core.Sequence;

namespace Comparation.Tests.Order.Tests
{
    public sealed class WorkAsExpected : Suite
    {
        private static IComparer<string> StringByLength => Of<string>().By(@string => @string.Length);

        private static IComparer<string> StringByReversedLength => Of<string>()
            .By(@string => @string.Length, Of<int>().Default.Invert());

        private static IComparer<string> ByLengthThenByFirstLetter => StringByLength.ThenBy(@string => @string[0]);

        private static IEnumerable<Named<Test>> Trivial()
        {
            var sut = Of<string>().Trivial;
            return new[]
            {
                Test("trivial 1", sut, "Hello", "world", Sign.Equal),
                Test("trivial 2", sut, "Quick", "fox", Sign.Equal)
            };
        }

        private static IEnumerable<Named<Test>> By() => new[]
        {
            Test("by", StringByLength, "Hello", "world", Sign.Equal),
            Test("by", StringByLength, "Quick", "fox", Sign.Greater),
            Test("by", StringByLength, "fox", "world", Sign.Less)
        };

        private static IEnumerable<Named<Test>> ByWithOrder() => new[]
        {
            Test("by with order", StringByReversedLength, "Hello", "world", Sign.Equal),
            Test("by with order", StringByReversedLength, "Quick", "fox", Sign.Less),
            Test("by with order", StringByReversedLength, "fox", "world", Sign.Greater)
        };

        private static IEnumerable<Named<Test>> ThenBy() => new[]
        {
            Test("then by", ByLengthThenByFirstLetter, "Hello", "Horde", Sign.Equal),
            Test("then by", ByLengthThenByFirstLetter, "Quick", "Apple", Sign.Greater),
            Test("then by", ByLengthThenByFirstLetter, "fox", "jar", Sign.Less)
        };

        private static IEnumerable<Named<Test>> ThenByWithOrder()
        {
            var sut = StringByLength.ThenBy(@string => @string[0], Of<char>().By(char.ToLowerInvariant));
            return new[]
            {
                Test("then by with order", sut, "Hello", "horde", Sign.Equal),
                Test("then by with order", sut, "Quick", "apple", Sign.Greater),
                Test("then by with order", sut, "Fox", "jar", Sign.Less)
            };
        }

        private static IEnumerable<Named<Test>> Composite()
        {
            var emptySut = Of<string>().AllowingEmptyComposite();
            var sut = Of<string>().Composite(
                StringByLength,
                Of<string>().By(@string => @string[0])
            );
            return new[]
            {
                Test("empty composite", emptySut, "Hello", "world", Sign.Equal),
                Test("composite", sut, "Hello", "Horde", Sign.Equal),
                Test("composite", sut, "Quick", "Apple", Sign.Greater),
                Test("composite", sut, "fox", "jar", Sign.Less)
            };
        }

        private static IEnumerable<Named<Test>> Sequence()
        {
            var sut = Of<string>().Sequence();
            return new[]
            {
                Test("sequence", sut, new[] {"Orange", "Apple", "Berry"}, new[] {"Orange", "Apple", "Berry"}, Sign.Equal),
                Test("sequence by length", sut, new[] {"Orange", "Apple", "Berry"}, new[] {"Orange", "Apple"}, Sign.Greater),
                Test("sequence by length", sut, new[] {"Orange", "Apple"}, new[] {"Orange", "Apple", "Berry"}, Sign.Less),
                Test("sequence by elements", sut, new[] {"Apple", "Orange", "Berry"}, new[] {"Orange", "Apple", "Berry"}, Sign.Less),
                Test("sequence by elements", sut, new[] {"Orange", "Apple", "Berry"}, new[] {"Berry", "Orange", "Apple"}, Sign.Greater)
            };
        }

        private static IEnumerable<Named<Test>> SequenceWithOrder()
        {
            var sut = Of<string>().Sequence(StringByLength);
            return new[]
            {
                Test("sequence with order", sut, new[] {"a", "bb", "ccc"}, new[] {"h", "oo", "bbb"}, Sign.Equal),
                Test("sequence with order by length", sut, new[] {"Orange", "Apple", "Berry"}, new[] {"Orange", "Apple"}, Sign.Greater),
                Test("sequence with order by length", sut, new[] {"Orange", "Apple"}, new[] {"Orange", "Apple", "Berry"}, Sign.Less),
                Test("sequence with order by elements", sut, new[] {"Apple", "Orange", "Berry"}, new[] {"Orange", "Apple", "Berry"}, Sign.Less),
                Test("sequence with order by elements", sut, new[] {"Orange", "Apple", "Berry"}, new[] {"Berry", "Orange", "Apple"}, Sign.Greater)
            };
        }

        public override IEnumerator<Named<Test>> GetEnumerator() => Concat(
            Trivial(),
            By(),
            ByWithOrder(),
            ThenBy(),
            ThenByWithOrder(),
            Composite(),
            Sequence(),
            SequenceWithOrder()
        ).GetEnumerator();

        private static Named<Test> Test<T>(string name, IComparer<T> sut, T a, T b, Sign expectation)
        {
            var test = new OrderShouldWorkAsExpected<T>(sut, a, b, expectation);
            var expectedVerdict = expectation.ToString("G").ToLowerInvariant();
            return new NamedByType<Test>($"{expectedVerdict} {name}", test);
        }
    }
}