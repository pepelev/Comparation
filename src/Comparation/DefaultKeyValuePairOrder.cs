using System.Collections.Generic;

namespace Comparation
{
    internal static class DefaultKeyValuePairOrder<TKey, TValue>
    {
        public static IComparer<KeyValuePair<TKey, TValue>> Singleton { get; } =
            Order.Of<KeyValuePair<TKey, TValue>>().Composite(
                Order.Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Key),
                Order.Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Value)
            );
    }
}