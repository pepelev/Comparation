using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class OrderExtensions
    {
        public static int ToInt(this Order.Sign sign) => (int) sign;

        public static Order.Sign Invert(this Order.Sign sign) => sign switch
        {
            Order.Sign.Less => Order.Sign.Greater,
            Order.Sign.Greater => Order.Sign.Less,
            _ => Order.Sign.Equal
        };

        public static Order.Sign Sign<T>(this IComparer<T> order, T a, T b) => Order.GetSign(order.Compare(a, b));

        public static T Max<T>(this IComparer<T> order, T a, T b) => order.Sign(a, b) == Order.Sign.Greater
            ? a
            : b;

        public static T Min<T>(this IComparer<T> order, T a, T b) => order.Sign(a, b) != Order.Sign.Greater
            ? a
            : b;

        public static IComparer<T> Invert<T>(this IComparer<T> order) => Comparer<T>.Create(
            (a, b) => order.Compare(b, a)
        );

        public static bool Equals<T>(this IComparer<T> order, T a, T b) => order.Sign(a, b) == Order.Sign.Equal;

        public static IComparer<TSubject> ThenBy<TSubject, TProjection>(
            this IComparer<TSubject> order,
            Func<TSubject, TProjection> projection) => order.ThenBy(projection, Comparer<TProjection>.Default);

        public static IComparer<TSubject> ThenBy<TSubject, TProjection>(
            this IComparer<TSubject> order,
            Func<TSubject, TProjection> projection,
            IComparer<TProjection> projectionOrder) =>
            Order<TSubject>.Singleton.Composite(
                order,
                Order<TSubject>.Singleton.By(projection, projectionOrder)
            );

        public static IComparer<TSubject> ThenUsing<TSubject>(
            this IComparer<TSubject> order,
            IComparer<TSubject> anotherOrder) =>
            Order<TSubject>.Singleton.Composite(
                order,
                anotherOrder
            );

        public static IComparer<IEnumerable<TSubject>> ForSequence<TSubject>(this IComparer<TSubject> itemOrder) =>
            Order<TSubject>.Singleton.Sequence(itemOrder);
    }
}