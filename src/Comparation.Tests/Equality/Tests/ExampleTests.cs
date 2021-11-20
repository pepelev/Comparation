using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Comparation.Tests.Core;
using Comparation.Tests.Equality.Aspects;

namespace Comparation.Tests.Equality.Tests
{
    public sealed class ExampleTests : IEnumerable<Named<Test>>
    {
        public IEnumerator<Named<Test>> GetEnumerator()
        {
            return Sequence.Concat(
                    EqualWaysOfComposition()
                ).GetEnumerator();
        }

        private static IEnumerable<Named<Test>> EqualWaysOfComposition()
        {
            var length = Comparation.Equality.Of<string>().By(@string => @string.Length);
            var firstLetter = Comparation.Equality.Of<string>().By(@string => @string[0]);
            var andUsing = length.AndUsing(firstLetter);
            var reversedAndUsing = firstLetter.AndUsing(length);
            var andBy = length.AndBy(@string => @string[0]);
            var reversedAndBy = firstLetter.AndBy(@string => @string.Length);
            var composite = Comparation.Equality.Of<string>().Composite(length, firstLetter);
            var reversedComposite = Comparation.Equality.Of<string>().Composite(firstLetter, length);

            var pairs = new (string Name, IEqualityComparer<string> Equality)[]
            {
                ("and-using", andUsing),
                ("reversed-and-using", reversedAndUsing),
                ("and-by", andBy),
                ("reversed-and-by", reversedAndBy),
                ("composite", composite),
                ("reversed-composite", reversedComposite)
            }.Pairwise();

            return
                from a in Strings.All
                from b in Strings.All
                from pair in pairs
                select new NamedByType<Test>(
                    $"{pair.Left.Name} vs {pair.Right.Name} | {a} - {b}",
                    new EqualityShouldBeEqual<string>(
                        pair.Left.Equality,
                        pair.Right.Equality,
                        a,
                        b
                    )
                );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}