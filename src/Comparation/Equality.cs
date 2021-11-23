using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class Equality
    {
        public static Equality<T> Of<T>(T sample) => Of<T>();
        public static Equality<T> Of<T>() => Equality<T>.Singleton;
        public static IEqualityComparer<T> ByReference<T>(T sample) where T : class => ByReference<T>();
        public static IEqualityComparer<T> ByReference<T>() where T : class => ReferenceEquality<T>.Singleton;

        public static IEqualityComparer<KeyValuePair<Key, Value>> ForKeyValuePair<Key, Value>() =>
            DefaultKeyValuePairEquality<Key, Value>.Singleton;

        public static IEqualityComparer<KeyValuePair<Key, Value>> ForKeyValuePair<Key, Value>(
            IEqualityComparer<Key> keyEquality,
            IEqualityComparer<Value> valueEquality) =>
            Of<KeyValuePair<Key, Value>>().Composite(
                Of<KeyValuePair<Key, Value>>().By(pair => pair.Key, keyEquality),
                Of<KeyValuePair<Key, Value>>().By(pair => pair.Value, valueEquality)
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

    public sealed class Equality<Subject>
    {
        internal static Equality<Subject> Singleton { get; } = new();

        public IEqualityComparer<Subject> Default => EqualityComparer<Subject>.Default;

        public IEqualityComparer<Subject> By<Projection>(Func<Subject, Projection> projection) =>
            By(projection, EqualityComparer<Projection>.Default);

        public IEqualityComparer<Subject> By<Projection>(
            Func<Subject, Projection> projection,
            IEqualityComparer<Projection> equality) =>
            new ProjectingEquality<Subject, Projection>(projection, equality);

        public IEqualityComparer<Subject> Composite(params IEqualityComparer<Subject>[] aspects) =>
            Composite(aspects as IReadOnlyCollection<IEqualityComparer<Subject>>);

        public IEqualityComparer<Subject> Composite(IReadOnlyCollection<IEqualityComparer<Subject>> aspects) =>
            new CompositeEquality<Subject>(aspects);

        public IEqualityComparer<IReadOnlyCollection<Subject?>> Collection() =>
            Collection(EqualityComparer<Subject>.Default);

        public IEqualityComparer<IReadOnlyCollection<Subject?>> Collection(IEqualityComparer<Subject> itemEquality) =>
            new CollectionEquality<Subject>(itemEquality);

        public IEqualityComparer<IReadOnlyCollection<Subject?>> Sequence() =>
            Sequence(EqualityComparer<Subject>.Default);

        public IEqualityComparer<IReadOnlyCollection<Subject?>> Sequence(IEqualityComparer<Subject> itemEquality) =>
            new SequenceEquality<Subject>(itemEquality);
    }
}