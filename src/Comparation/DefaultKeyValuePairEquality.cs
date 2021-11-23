using System.Collections.Generic;

namespace Comparation
{
    internal static class DefaultKeyValuePairEquality<Key, Value>
    {
        public static IEqualityComparer<KeyValuePair<Key, Value>> Singleton { get; } =
            Equality.Of<KeyValuePair<Key, Value>>().Composite(
                Equality.Of<KeyValuePair<Key, Value>>().By(pair => pair.Key),
                Equality.Of<KeyValuePair<Key, Value>>().By(pair => pair.Value)
            );
    }
}