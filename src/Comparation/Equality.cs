using System;
using System.Collections.Generic;
using System.Linq;

namespace Comparation
{
    public static class Equality
    {
        public static Equality<T> Of<T>(T sample) => Of<T>();
        public static Equality<T> Of<T>() => Equality<T>.Singleton;
        public static IEqualityComparer<T> ByReference<T>(T sample) where T : class => ByReference<T>();
        public static IEqualityComparer<T> ByReference<T>() where T : class => ReferenceEquality<T>.Singleton;

        public static IEqualityComparer<KeyValuePair<TKey, TValue>> ForKeyValuePair<TKey, TValue>() =>
            DefaultKeyValuePairEquality<TKey, TValue>.Singleton;

        public static IEqualityComparer<KeyValuePair<TKey, TValue>> ForKeyValuePair<TKey, TValue>(
            IEqualityComparer<TKey> keyEquality,
            IEqualityComparer<TValue> valueEquality) =>
            Of<KeyValuePair<TKey, TValue>>().Composite(
                Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Key, keyEquality),
                Of<KeyValuePair<TKey, TValue>>().By(pair => pair.Value, valueEquality)
            );

        public static IEqualityComparer<(T1, T2)> ForTuple<T1, T2>(IEqualityComparer<T1> equality1, IEqualityComparer<T2> equality2) =>
            Of<(T1, T2)>().Composite(
                Of<(T1, T2)>().By(tuple => tuple.Item1, equality1),
                Of<(T1, T2)>().By(tuple => tuple.Item2, equality2)
            );

        public static IEqualityComparer<(T1, T2, T3)> ForTuple<T1, T2, T3>(
            IEqualityComparer<T1> equality1,
            IEqualityComparer<T2> equality2,
            IEqualityComparer<T3> equality3) =>
            Of<(T1, T2, T3)>().Composite(
                Of<(T1, T2, T3)>().By(tuple => tuple.Item1, equality1),
                Of<(T1, T2, T3)>().By(tuple => tuple.Item2, equality2),
                Of<(T1, T2, T3)>().By(tuple => tuple.Item3, equality3)
            );

        public static IEqualityComparer<(T1, T2, T3, T4)> ForTuple<T1, T2, T3, T4>(
            IEqualityComparer<T1> equality1,
            IEqualityComparer<T2> equality2,
            IEqualityComparer<T3> equality3,
            IEqualityComparer<T4> equality4) =>
            Of<(T1, T2, T3, T4)>().Composite(
                Of<(T1, T2, T3, T4)>().By(tuple => tuple.Item1, equality1),
                Of<(T1, T2, T3, T4)>().By(tuple => tuple.Item2, equality2),
                Of<(T1, T2, T3, T4)>().By(tuple => tuple.Item3, equality3),
                Of<(T1, T2, T3, T4)>().By(tuple => tuple.Item4, equality4)
            );
    }

    public sealed class Equality<TSubject>
    {
        internal static Equality<TSubject> Singleton { get; } = new();

        public IEqualityComparer<TSubject> Default => EqualityComparer<TSubject>.Default;

        public IEqualityComparer<TSubject> By<TProjection>(Func<TSubject, TProjection?> projection) =>
            By(projection, EqualityComparer<TProjection>.Default);

        public IEqualityComparer<TSubject> By<TProjection>(
            Func<TSubject, TProjection?> projection,
            IEqualityComparer<TProjection> equality) =>
            new ProjectingEquality<TSubject, TProjection>(projection, equality);

        public IEqualityComparer<TSubject> Composite(params IEqualityComparer<TSubject>[] aspects)
        {
            if (aspects.Length == 1)
            {
                return aspects[0];
            }

            if (aspects.Length == 2)
            {
                return new CompoundEquality<TSubject>(aspects[0], aspects[1]);
            }

            return new CompositeEquality<TSubject>(aspects);
        }

        public IEqualityComparer<TSubject> Composite(IReadOnlyCollection<IEqualityComparer<TSubject>> aspects) =>
            Composite(aspects.ToArray());

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Collection() =>
            Collection(EqualityComparer<TSubject>.Default);

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Collection(IEqualityComparer<TSubject> itemEquality) =>
            new CollectionEquality<TSubject>(itemEquality);

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Sequence() =>
            Sequence(EqualityComparer<TSubject>.Default);

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Sequence(IEqualityComparer<TSubject> itemEquality) =>
            new SequenceEquality<TSubject>(itemEquality);

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Set() =>
            Set(EqualityComparer<TSubject>.Default);

        public IEqualityComparer<IReadOnlyCollection<TSubject?>> Set(IEqualityComparer<TSubject> itemEquality) =>
            new SetEquality<TSubject>(itemEquality);
    }
}