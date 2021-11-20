using System.Collections.Generic;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class TreatTreatsNullNotEqualToObject : Suite
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

            yield return Test("by", @by, "Hunter");
            yield return Test("by reference", byReference, "Cerberus");
            yield return Test("by with equality", byWithEquality, "Image");
            yield return Test("and by", andBy, "Mountain");
            yield return Test("and by with equality", andByWithEquality, "Cup");
            yield return Test("and using", andUsing, "Seal");
            yield return Test("composite", composite, "Deal");
            yield return Test("sequence", sequence, new[] {"Cow"});
            yield return Test("sequence with equality", sequenceWithEquality, new[] {"Sneak"});
            yield return Test("collection", collection, new[] {"Dog"});
            yield return Test("collection with equality", collectionWithEquality, new[] {"Cat"});
        }

        private static Named<Test> Test<T>(string name, IEqualityComparer<T> sut, T argument) where T : class
            => new NamedByType<Test>(
                name,
                new EqualityShouldTreatsNullNotEqualToObject<T>(sut, argument)
            );
    }
}