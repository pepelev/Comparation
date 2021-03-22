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
    }

    public sealed class Equality<Subject>
    {
        internal static Equality<Subject> Singleton { get; } = new();

        public IEqualityComparer<Subject> By<Projection>(Func<Subject, Projection> projection) =>
            By(projection, EqualityComparer<Projection>.Default);

        public IEqualityComparer<Subject> By<Projection>(
            Func<Subject, Projection> projection,
            IEqualityComparer<Projection> equality)
        {
            return new ProjectingEquality<Subject, Projection>(projection, equality);
        }

        public IEqualityComparer<Subject> Composite(params IEqualityComparer<Subject>[] aspects) =>
            new CompositeEquality<Subject>(aspects);

        public IEqualityComparer<Subject> Composite(IReadOnlyCollection<IEqualityComparer<Subject>> aspects) =>
            new CompositeEquality<Subject>(aspects);

        public IEqualityComparer<IReadOnlyCollection<Subject>> Collection() =>
            Collection(EqualityComparer<Subject>.Default);

        public IEqualityComparer<IReadOnlyCollection<Subject>> Collection(IEqualityComparer<Subject> itemEquality) =>
            new CollectionEquality<Subject>(itemEquality);

        public IEqualityComparer<IReadOnlyCollection<Subject>> Sequence() =>
            Sequence(EqualityComparer<Subject>.Default);

        public IEqualityComparer<IReadOnlyCollection<Subject>> Sequence(IEqualityComparer<Subject> itemEquality) =>
            new SequenceEquality<Subject>(itemEquality);
    }
}