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

        public static IComparer<Subject> ThenBy<Subject, Projection>(
            this IComparer<Subject> order,
            Func<Subject, Projection> projection) => order.ThenBy(projection, Comparer<Projection>.Default);

        public static IComparer<Subject> ThenBy<Subject, Projection>(
            this IComparer<Subject> order,
            Func<Subject, Projection> projection,
            IComparer<Projection> projectionOrder) =>
            Order<Subject>.Singleton.Composite(
                order,
                Order<Subject>.Singleton.By(projection, projectionOrder)
            );

        public static IComparer<Subject> ThenUsing<Subject>(
            this IComparer<Subject> order,
            IComparer<Subject> anotherOrder) =>
            Order<Subject>.Singleton.Composite(
                order,
                anotherOrder
            );

        public static IComparer<IEnumerable<Subject>> ForSequence<Subject>(this IComparer<Subject> itemOrder) =>
            Order<Subject>.Singleton.Sequence(itemOrder);
    }
}