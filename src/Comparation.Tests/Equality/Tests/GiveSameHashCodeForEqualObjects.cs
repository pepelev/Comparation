using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class GiveSameHashCodeForEqualObjects : Suite
    {
        private static IEqualityComparer<char> CharCaseInsensitively => Comparation.Equality.Of<char>().By(char.ToLowerInvariant);

        public override IEnumerator<Named<Test>> GetEnumerator()
        {
            var by = Comparation.Equality.Of<string>().By(@string => @string.Length);
            var byReference = Comparation.Equality.ByReference<string>();
            var byWithEquality = Comparation.Equality.Of<string>().By(
                @string => @string[0],
                CharCaseInsensitively
            );
            var andBy = by.AndBy(@string => @string[0]);
            var andByWithEquality = by.AndBy(@string => @string[0], CharCaseInsensitively);
            var anotherBy = Comparation.Equality.Of<string>().By(@string => @string[0]);
            var andUsing = by.AndUsing(anotherBy);
            var composite = Comparation.Equality.Of<string>().Composite(by, anotherBy);
            var sequence = Comparation.Equality.Of<string>().Sequence();
            var sequenceWithEquality = Comparation.Equality.Of<string>().Sequence(by);
            var collection = Comparation.Equality.Of<string>().Collection();
            var collectionWithEquality = Comparation.Equality.Of<string>().Collection(by);

            var cases = Sequence.Concat(
                Cross("by", by, Strings.All),
                Cross("by reference", byReference, Strings.All),
                Cross("by with equality", byWithEquality, Strings.All),
                Cross("and by", andBy, Strings.All),
                Cross("and by with equality", andByWithEquality, Strings.All),
                Cross("and using", andUsing, Strings.All),
                Cross("composite", composite, Strings.All),
                Cross("sequence", sequence, Strings.Collections),
                Cross("sequence with equality", sequenceWithEquality, Strings.Collections),
                Cross("collection", collection, Strings.Collections),
                Cross("collection with equality", collectionWithEquality, Strings.Collections)
            );
            return cases.GetEnumerator();
        }

        private static IEnumerable<Named<Test>> Cross<T>(string prefix, IEqualityComparer<T> sut, IEnumerable<T?> items)
        {
            var list = items.ToList();
            return
                from a in list
                from b in list
                select new NamedByType<Test>(
                    $"{prefix} | {a} - {b}",
                    new EqualityShouldGiveSameHashCodeForEqualObjects<T>(sut, a, b)
                );
        }
    }
}