using System.Collections.Generic;

namespace Comparation
{
    internal static class DefaultKeyValuePairOrder<Key, Value>
    {
        public static IComparer<KeyValuePair<Key, Value>> Singleton { get; } =
            Order.Of<KeyValuePair<Key, Value>>().Composite(
                Order.Of<KeyValuePair<Key, Value>>().By(pair => pair.Key),
                Order.Of<KeyValuePair<Key, Value>>().By(pair => pair.Value)
            );
    }
}