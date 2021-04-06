using System.Collections.Generic;
using System.Linq;

namespace Comparation.Tests.Core
{
    public static class Sequence
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sequences) =>
            sequences.SelectMany(item => item);
    }
}