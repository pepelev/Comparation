using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class EqualityExtensions
    {
        public static IEqualityComparer<Subject> AndBy<Subject, Projection>(
            this IEqualityComparer<Subject> equality,
            Func<Subject, Projection> projection) =>
            equality.AndBy(projection, EqualityComparer<Projection>.Default);

        public static IEqualityComparer<Subject> AndBy<Subject, Projection>(
            this IEqualityComparer<Subject> equality,
            Func<Subject, Projection> projection,
            IEqualityComparer<Projection> projectionEquality) =>
            Equality<Subject>.Singleton.Composite(
                equality,
                Equality<Subject>.Singleton.By(projection, projectionEquality)
            );

        public static IEqualityComparer<Subject> AndUsing<Subject>(
            this IEqualityComparer<Subject> equality,
            IEqualityComparer<Subject> anotherEquality) =>
            Equality<Subject>.Singleton.Composite(equality, anotherEquality);

        public static IEqualityComparer<IReadOnlyCollection<Subject>> ForCollection<Subject>(
            this IEqualityComparer<Subject> itemEquality) =>
            Equality<Subject>.Singleton.Collection(itemEquality);

        public static IEqualityComparer<IReadOnlyCollection<Subject>> ForSequence<Subject>(
            this IEqualityComparer<Subject> itemEquality) =>
            Equality<Subject>.Singleton.Sequence(itemEquality);
    }
}