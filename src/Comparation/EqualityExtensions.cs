using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class EqualityExtensions
    {
        public static IEqualityComparer<TSubject> AndBy<TSubject, TProjection>(
            this IEqualityComparer<TSubject> equality,
            Func<TSubject, TProjection> projection) =>
            equality.AndBy(projection, EqualityComparer<TProjection>.Default);

        public static IEqualityComparer<TSubject> AndBy<TSubject, TProjection>(
            this IEqualityComparer<TSubject> equality,
            Func<TSubject, TProjection> projection,
            IEqualityComparer<TProjection> projectionEquality) =>
            Equality<TSubject>.Singleton.Composite(
                equality,
                Equality<TSubject>.Singleton.By(projection, projectionEquality)
            );

        public static IEqualityComparer<TSubject> AndUsing<TSubject>(
            this IEqualityComparer<TSubject> equality,
            IEqualityComparer<TSubject> anotherEquality) =>
            Equality<TSubject>.Singleton.Composite(equality, anotherEquality);

        public static IEqualityComparer<IReadOnlyCollection<TSubject>> ForCollection<TSubject>(
            this IEqualityComparer<TSubject> itemEquality) =>
            Equality<TSubject>.Singleton.Collection(itemEquality);

        public static IEqualityComparer<IReadOnlyCollection<TSubject>> ForSequence<TSubject>(
            this IEqualityComparer<TSubject> itemEquality) =>
            Equality<TSubject>.Singleton.Sequence(itemEquality);

        public static IEqualityComparer<IReadOnlyCollection<TSubject>> ForSet<TSubject>(
            this IEqualityComparer<TSubject> itemEquality) =>
            Equality<TSubject>.Singleton.Set(itemEquality);
    }
}