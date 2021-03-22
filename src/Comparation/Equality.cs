using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class Equality
    {
        public static Equality<T> Of<T>() => Equality<T>.Singleton;
        public static Equality<T> Of<T>(T sample) => Of<T>();
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
    }
}