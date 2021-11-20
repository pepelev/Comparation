using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;
using static Comparation.Equality;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class Transitive : Suite
    {
        private static IEqualityComparer<char> CharCaseInsensitively => Of<char>().By(char.ToLowerInvariant);

        public override IEnumerator<Named<Test>> GetEnumerator()
        {
            var by = Of<string>().By(@string => @string.Length);
            var byReference = ByReference<string>();
            var byWithEquality = Of<string>().By(
                @string => @string[0],
                CharCaseInsensitively
            );
            var andBy = by.AndBy(@string => @string[0]);
            var andByWithEquality = by.AndBy(@string => @string[0], CharCaseInsensitively);
            var anotherBy = Of<string>().By(@string => @string[0]);
            var andUsing = by.AndUsing(anotherBy);
            var composite = Of<string>().Composite(by, anotherBy);
            var sequence = Of<string>().Sequence();
            var sequenceWithEquality = Of<string>().Sequence(by);
            var collection = Of<string>().Collection();
            var collectionWithEquality = Of<string>().Collection(by);

            return Sequence.Concat(
                Cross("by", @by, Strings.All),
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
            ).GetEnumerator();
        }

        private static IEnumerable<Named<Test>> Cross<T>(
            string name,
            IEqualityComparer<T> sut,
            IEnumerable<T?> arguments)
        {
            var list = arguments.ToList();
            return
                from a in list
                from b in list
                from c in list
                select new NamedByType<Test>(
                    $"{name} | {a} - {b} - {c}",
                    new EqualityShouldBeTransitive<T>(sut, a, b, c)
                );
        }
    }
}