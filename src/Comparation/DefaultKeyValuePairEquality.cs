using System.Collections.Generic;

namespace Comparation
{
    internal static class DefaultKeyValuePairEquality<TKey, TValue>
    {
        public static IEqualityComparer<KeyValuePair<TKey, TValue>> Singleton { get; } =
            Equality.Of<KeyValuePair<TKey, TValue>>().Composite(
                Equality.Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Key),
                Equality.Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Value)
            );
    }
}