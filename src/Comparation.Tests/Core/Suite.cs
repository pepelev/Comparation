using System.Collections;
using System.Collections.Generic;

namespace Comparation.Tests.Core
{
    public abstract class Suite : IEnumerable<Named<Test>>
    {
        public abstract IEnumerator<Named<Test>> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}