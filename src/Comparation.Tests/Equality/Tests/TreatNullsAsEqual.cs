using System.Collections.Generic;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;
using static Comparation.Equality;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class TreatNullsAsEqual : Suite
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

            yield return Test("by", by);
            yield return Test("by reference", byReference);
            yield return Test("by with equality", byWithEquality);
            yield return Test("and by", andBy);
            yield return Test("and by with equality", andByWithEquality);
            yield return Test("and using", andUsing);
            yield return Test("composite", composite);
            yield return Test("sequence", sequence);
            yield return Test("sequence with equality", sequenceWithEquality);
            yield return Test("collection", collection);
            yield return Test("collection with equality", collectionWithEquality);
        }

        private static Named<Test> Test<T>(string name, IEqualityComparer<T> sut) where T : class
            => new NamedByType<Test>(
                name,
                new EqualityShouldTreatsNullsAsEqual<T>(sut)
            );
    }
}