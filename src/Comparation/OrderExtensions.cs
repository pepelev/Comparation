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

#if NET6_0_OR_GREATER
        public static T Max<T>(this IComparer<T> order, T first, params ReadOnlySpan<T> others)
        {
            var max = first;
            foreach (var another in others)
            {
                max = order.Max(another, max);
            }

            return max;
        }

        public static T MaxOrThrow<T>(this IComparer<T> order, params ReadOnlySpan<T> items)
        {
            if (items.Length == 0)
            {
                throw new ArgumentException("is empty", paramName: nameof(items));
            }

            var max = items[0];
            for (var i = 1; i < items.Length; i++)
            {
                var item = items[i];
                max = order.Max(max, item);
            }

            return max;
        }

        public static int MaxIndexOrThrow<T>(this IComparer<T> order, params ReadOnlySpan<T> items)
        {
            if (items.Length == 0)
            {
                throw new ArgumentException("is empty", paramName: nameof(items));
            }

            var max = items[0];
            var maxIndex = 0;
            for (var i = 1; i < items.Length; i++)
            {
                var item = items[i];
                if (order.Sign(item, max) == Order.Sign.Greater)
                {
                    max = item;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }
#endif

        public static T Min<T>(this IComparer<T> order, T a, T b) => order.Sign(a, b) != Order.Sign.Greater
            ? a
            : b;

#if NET6_0_OR_GREATER
        public static T Min<T>(this IComparer<T> order, T first, params ReadOnlySpan<T> others)
        {
            var min = first;
            foreach (var another in others)
            {
                min = order.Min(min, another);
            }

            return min;
        }

        public static T MinOrThrow<T>(this IComparer<T> order, params ReadOnlySpan<T> items)
        {
            if (items.Length == 0)
            {
                throw new ArgumentException("is empty", paramName: nameof(items));
            }

            var min = items[0];
            for (var i = 1; i < items.Length; i++)
            {
                var item = items[i];
                min = order.Min(min, item);
            }

            return min;
        }

        public static int MinIndexOrThrow<T>(this IComparer<T> order, params ReadOnlySpan<T> items)
        {
            if (items.Length == 0)
            {
                throw new ArgumentException("is empty", paramName: nameof(items));
            }

            var min = items[0];
            var minIndex = 0;
            for (var i = 1; i < items.Length; i++)
            {
                var item = items[i];
                if (order.Sign(item, min) == Order.Sign.Less)
                {
                    min = item;
                    minIndex = i;
                }
            }

            return minIndex;
        }
#endif

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