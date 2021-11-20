using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;
using static Comparation.Equality;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class Equal : Suite
    {
        private static IEqualityComparer<string> FirstLetter => Of<string>().By(@string => @string[0]);
        private static IEqualityComparer<string> Length => Of<string>().By(@string => @string.Length);

        private static IEnumerable<Named<Test>> ObjectEqualities
        {
            get
            {
                var andUsing = Length.AndUsing(FirstLetter);
                var reversedAndUsing = FirstLetter.AndUsing(Length);
                var andBy = Length.AndBy(@string => @string[0]);
                var reversedAndBy = FirstLetter.AndBy(@string => @string.Length);
                var composite = Of<string>().Composite(Length, FirstLetter);
                var reversedComposite = Of<string>().Composite(FirstLetter, Length);

                var objectEqualities = new (string Name, IEqualityComparer<string> Equality)[]
                {
                    ("and-using", andUsing),
                    ("reversed-and-using", reversedAndUsing),
                    ("and-by", andBy),
                    ("reversed-and-by", reversedAndBy),
                    ("composite", composite),
                    ("reversed-composite", reversedComposite)
                };
                return Tests(objectEqualities, Strings.All);
            }
        }

        private static IEnumerable<Named<Test>> SequenceEqualities => Tests(
            new[]
            {
                ("sequence-equality-method", Of<string>().Sequence(Length)),
                ("sequence-extension-method", Length.ForSequence())
            },
            Strings.NonNullableCollections
        );

        private static IEnumerable<Named<Test>> CollectionEqualities => Tests(
            new[]
            {
                ("collection-equality-method", Of<string>().Collection(Length)),
                ("collection-extension-method", Length.ForCollection())
            },
            Strings.NonNullableCollections
        );

        public override IEnumerator<Named<Test>> GetEnumerator() => Sequence.Concat(
            ObjectEqualities,
            SequenceEqualities,
            CollectionEqualities
        ).GetEnumerator();

        private static IEnumerable<Named<Test>> Tests<T>(
            (string Name, IEqualityComparer<T> Equality)[] equalEqualities,
            IEnumerable<T?> arguments)
        {
            var list = arguments.ToList();
            return
                from a in list
                from b in list
                from pair in equalEqualities.Pairwise()
                select new NamedByType<Test>(
                    $"{pair.Left.Name} vs {pair.Right.Name} | {a} - {b}",
                    new EqualityShouldBeEqual<T>(
                        pair.Left.Equality,
                        pair.Right.Equality,
                        a,
                        b
                    )
                );
        }
    }
}