// using System.Collections.Generic;
// using FluentAssertions;
// using NUnit.Framework;
//
// namespace Comparation.Tests
// {
//     public sealed class EqualityShould
//     {
//         public static TestCaseData[] EqualityCases => new[]
//         {
//             new TestCaseData(StringByLength, "Hello", "world").Returns(true).SetName("positive single by"),
//             new TestCaseData(StringByLength, "Quick", "fox").Returns(false).SetName("negative single by"),
//
//             new TestCaseData(StringByReference, "Hello", "Hello").Returns(true).SetName("positive by reference (interning)"),
//             new TestCaseData(StringByReference, "Quick", string.Concat("Qui", "ck")).Returns(false).SetName("negative by reference"),
//
//             new TestCaseData(
//                 StringByFirstLetterCaseInsensitively,
//                 "Firmware",
//                 "fox"
//             ).Returns(true).SetName("positive single by with equality"),
//             new TestCaseData(
//                 StringByFirstLetterCaseInsensitively,
//                 "Yeti",
//                 "Bigfoot"
//             ).Returns(false).SetName("negative single by with equality"),
//
//             new TestCaseData(
//                 StringByLengthAndFirstLetter,
//                 "Brown",
//                 "Beast"
//             ).Returns(true).SetName("positive and-by"),
//             new TestCaseData(
//                 StringByLengthAndFirstLetter,
//                 "Apple",
//                 "Alpine"
//             ).Returns(false).SetName("negative and-by on first field"),
//             new TestCaseData(
//                 StringByLengthAndFirstLetter,
//                 "Fear",
//                 "Calm"
//             ).Returns(false).SetName("negative and-by on second field"),
//
//             new TestCaseData(
//                 StringByLengthAndFirstLetterCaseInsensitively,
//                 "Brown",
//                 "blast"
//             ).Returns(true).SetName("positive and-by with equality"),
//             new TestCaseData(
//                 StringByLengthAndFirstLetterCaseInsensitively,
//                 "Apple",
//                 "Alpine"
//             ).Returns(false).SetName("negative and-by with equality on first field"),
//             new TestCaseData(
//                 StringByLengthAndFirstLetterCaseInsensitively,
//                 "Fear",
//                 "Calm"
//             ).Returns(false).SetName("negative and-by with equality on second field"),
//
//             new TestCaseData(
//                 StringByLengthAndUsingFirstLetterCaseInsensitively,
//                 "Brown",
//                 "blast"
//             ).Returns(true).SetName("positive and-by with equality"),
//             new TestCaseData(
//                 StringByLengthAndUsingFirstLetterCaseInsensitively,
//                 "Apple",
//                 "Alpine"
//             ).Returns(false).SetName("negative and-by with equality on first field"),
//             new TestCaseData(
//                 StringByLengthAndUsingFirstLetterCaseInsensitively,
//                 "Fear",
//                 "Calm"
//             ).Returns(false).SetName("negative and-by with equality on second field"),
//
//             new TestCaseData(
//                 StringCompositeByLengthFirstLetterCaseInsensitively,
//                 "Brown",
//                 "blast"
//             ).Returns(true).SetName("positive and-by with equality"),
//             new TestCaseData(
//                 StringCompositeByLengthFirstLetterCaseInsensitively,
//                 "Apple",
//                 "Alpine"
//             ).Returns(false).SetName("negative and-by with equality on first field"),
//             new TestCaseData(
//                 StringCompositeByLengthFirstLetterCaseInsensitively,
//                 "Fear",
//                 "Calm"
//             ).Returns(false).SetName("negative and-by with equality on second field"),
//
//             new TestCaseData(
//                 StringCompositeCollectionByLengthFirstLetterCaseInsensitively,
//                 "Brown",
//                 "blast"
//             ).Returns(true).SetName("positive and-by with equality"),
//             new TestCaseData(
//                 StringCompositeCollectionByLengthFirstLetterCaseInsensitively,
//                 "Apple",
//                 "Alpine"
//             ).Returns(false).SetName("negative and-by with equality on first field"),
//             new TestCaseData(
//                 StringCompositeCollectionByLengthFirstLetterCaseInsensitively,
//                 "Fear",
//                 "Calm"
//             ).Returns(false).SetName("negative and-by with equality on second field")
//         };
//
//         private static IEqualityComparer<string> StringByLength => Comparation.Equality.Of<string>().By(@string => @string.Length);
//         private static IEqualityComparer<string> StringByReference => Comparation.Equality.ByReference<string>();
//
//         private static IEqualityComparer<string> StringByFirstLetterCaseInsensitively => Comparation.Equality.Of<string>().By(
//             @string => @string[0],
//             CharCaseInsensitively
//         );
//
//         private static IEqualityComparer<char> CharCaseInsensitively => Comparation.Equality.Of<char>().By(char.ToLowerInvariant);
//
//         private static IEqualityComparer<string> StringByLengthAndFirstLetter =>
//             StringByLength.AndBy(@string => @string[0]);
//
//         private static IEqualityComparer<string> StringByLengthAndFirstLetterCaseInsensitively =>
//             StringByLength.AndBy(@string => @string[0], CharCaseInsensitively);
//
//         private static IEqualityComparer<string> StringByLengthAndUsingFirstLetterCaseInsensitively =>
//             StringByLength.AndUsing(StringByFirstLetterCaseInsensitively);
//
//         private static IEqualityComparer<string> StringCompositeByLengthFirstLetterCaseInsensitively =>
//             Comparation.Equality.Of<string>().Composite(
//                 StringByLength,
//                 StringByFirstLetterCaseInsensitively
//             );
//
//         private static IEqualityComparer<string> StringCompositeCollectionByLengthFirstLetterCaseInsensitively =>
//             Comparation.Equality.Of<string>().Composite(
//                 new List<IEqualityComparer<string>>
//                 {
//                     StringByLength,
//                     StringByFirstLetterCaseInsensitively
//                 }
//             );
//
//         private static IEqualityComparer<IReadOnlyCollection<string>> StringCollection =>
//             Comparation.Equality.Of<string>().Collection();
//
//         private static IEqualityComparer<IReadOnlyCollection<string>> StringSequence =>
//             Comparation.Equality.Of<string>().Sequence();
//
//         public static TestCaseData[] Equalities => new[]
//         {
//             new TestCaseData(StringByLength).SetName(nameof(StringByLength)),
//             new TestCaseData(StringByReference).SetName(nameof(StringByReference)),
//             new TestCaseData(StringByFirstLetterCaseInsensitively)
//                 .SetName(nameof(StringByFirstLetterCaseInsensitively)),
//             new TestCaseData(StringByLengthAndFirstLetter).SetName(nameof(StringByLengthAndFirstLetter)),
//             new TestCaseData(StringByLengthAndFirstLetterCaseInsensitively)
//                 .SetName(nameof(StringByLengthAndFirstLetterCaseInsensitively)),
//             new TestCaseData(StringByLengthAndUsingFirstLetterCaseInsensitively)
//                 .SetName(nameof(StringByLengthAndUsingFirstLetterCaseInsensitively)),
//             new TestCaseData(StringCompositeByLengthFirstLetterCaseInsensitively)
//                 .SetName(nameof(StringCompositeByLengthFirstLetterCaseInsensitively)),
//             new TestCaseData(StringCompositeCollectionByLengthFirstLetterCaseInsensitively)
//                 .SetName(nameof(StringCompositeCollectionByLengthFirstLetterCaseInsensitively))
//         };
//
//         public static TestCaseData[] EqualitiesWithArgument => new[]
//         {
//             new TestCaseData(StringByLength, "Dog").SetName(nameof(StringByLength)),
//             new TestCaseData(StringByReference, "And").SetName(nameof(StringByReference)),
//             new TestCaseData(StringByFirstLetterCaseInsensitively, "Cat")
//                 .SetName(nameof(StringByFirstLetterCaseInsensitively)),
//             new TestCaseData(StringByLengthAndFirstLetter, "Are").SetName(nameof(StringByLengthAndFirstLetter)),
//             new TestCaseData(StringByLengthAndFirstLetterCaseInsensitively, "Friends")
//                 .SetName(nameof(StringByLengthAndFirstLetterCaseInsensitively)),
//             new TestCaseData(StringByLengthAndUsingFirstLetterCaseInsensitively, "Believe")
//                 .SetName(nameof(StringByLengthAndUsingFirstLetterCaseInsensitively)),
//             new TestCaseData(StringCompositeByLengthFirstLetterCaseInsensitively, "Me")
//                 .SetName(nameof(StringCompositeByLengthFirstLetterCaseInsensitively)),
//             new TestCaseData(StringCompositeCollectionByLengthFirstLetterCaseInsensitively, "Pal")
//                 .SetName(nameof(StringCompositeCollectionByLengthFirstLetterCaseInsensitively))
//         };
//
//         [Test]
//         [TestCaseSource(nameof(EqualityCases))]
//         public bool DefineEquality<T>(IEqualityComparer<T> equality, T a, T b) => equality.Equals(a, b);
//
//         [Test]
//         [TestCase(new string[0], new string[0], ExpectedResult = true)]
//         [TestCase(new[] {"Dog"}, new string[0], ExpectedResult = false)]
//         [TestCase(new[] {"Dog"}, new[] {"Dog"}, ExpectedResult = true)]
//         [TestCase(new[] {"Dog"}, new[] {"Dog", "Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Dog"}, new[] {"Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Dog", "Cat"}, new[] {"Dog", "Cat"}, ExpectedResult = true)]
//         [TestCase(new[] {"Cat", "Dog"}, new[] {"Dog", "Cat"}, ExpectedResult = true)]
//         [TestCase(new[] {"Cat", "Dog", "Dog"}, new[] {"Dog", "Cat", "Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Cat", "Dog", "Dog"}, new[] {"Dog", "Cat", "Dog"}, ExpectedResult = true)]
//         public bool DefineEqualityOnCollections(string[] a, string[] b) => StringCollection.Equals(a, b);
//
//         [Test]
//         [TestCase(new string[0], new string[0], ExpectedResult = true)]
//         [TestCase(new[] {"Dog"}, new string[0], ExpectedResult = false)]
//         [TestCase(new[] {"Dog"}, new[] {"Dog"}, ExpectedResult = true)]
//         [TestCase(new[] {"Dog"}, new[] {"Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Dog"}, new[] {"Dog", "Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Dog", "Cat"}, new[] {"Dog", "Cat"}, ExpectedResult = true)]
//         [TestCase(new[] {"Cat", "Dog"}, new[] {"Dog", "Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Cat", "Dog", "Dog"}, new[] {"Dog", "Cat", "Cat"}, ExpectedResult = false)]
//         [TestCase(new[] {"Cat", "Dog", "Dog"}, new[] {"Dog", "Cat", "Dog"}, ExpectedResult = false)]
//         [TestCase(new[] {"Dog", "Cat", "Dog"}, new[] {"Dog", "Cat", "Dog"}, ExpectedResult = true)]
//         public bool DefineEqualityOnSequences(string[] a, string[] b) => StringSequence.Equals(a, b);
//
//         [Test]
//         [TestCaseSource(nameof(EqualitiesWithArgument))]
//         public void WorkWithFirstArgumentNull<T>(IEqualityComparer<T> equality, T b) where T : class
//         {
//             equality.Equals(null, b).Should().BeFalse();
//         }
//
//         [Test]
//         [TestCaseSource(nameof(EqualitiesWithArgument))]
//         public void WorkWithSecondArgumentNull<T>(IEqualityComparer<T> equality, T a) where T : class
//         {
//             equality.Equals(a, null).Should().BeFalse();
//         }
//
//         [Test]
//         [TestCaseSource(nameof(Equalities))]
//         public void TreatBothNullsAsEqual(IEqualityComparer<string> equality)
//         {
//             equality.Equals(null, null).Should().BeTrue();
//         }
//     }
// }