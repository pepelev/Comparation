using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class Order
    {
        public enum Sign : sbyte
        {
            Less = -1,
            Equal = 0,
            Greater = 1
        }

        public static Sign GetSign(int comparisonResult) => comparisonResult switch
        {
            0 => Sign.Equal,
            < 0 => Sign.Less,
            > 0 => Sign.Greater
        };

        public static Order<T> Of<T>() => Order<T>.Singleton;
        public static Order<T> Of<T>(T sample) => Of<T>();
    }

    public sealed class Order<Subject>
    {
        internal static Order<Subject> Singleton { get; } = new();

        public IComparer<Subject> Trivial { get; } = Comparer<Subject>.Create(
            (_, _) => 0
        );

        public IComparer<Subject> Default => Comparer<Subject>.Default;

        public IComparer<Subject> By<Projection>(Func<Subject, Projection> projection) =>
            By(projection, Comparer<Projection>.Default);

        public IComparer<Subject> By<Projection>(Func<Subject, Projection> projection, IComparer<Projection> order) =>
            Comparer<Subject>.Create(
                (a, b) => order.Compare(
                    projection(a),
                    projection(b)
                )
            );

        public IComparer<Subject> Composite(params IComparer<Subject>[] aspects) =>
            Composite(aspects as IReadOnlyCollection<IComparer<Subject>>);

        public IComparer<Subject> Composite(IReadOnlyCollection<IComparer<Subject>> aspects)
        {
            if (aspects.Count == 0)
            {
                throw new ArgumentException(
                    "Aspects collection must have elements. If you intended to create " +
                    "trivial order which treats all values equal, use the " +
                    "Trivial property or the AllowingEmptyComposite method.",
                    nameof(aspects)
                );
            }

            return AllowingEmptyComposite(aspects);
        }

        public IComparer<Subject> AllowingEmptyComposite(params IComparer<Subject>[] aspects) =>
            AllowingEmptyComposite(aspects as IReadOnlyCollection<IComparer<Subject>>);

        public IComparer<Subject> AllowingEmptyComposite(IReadOnlyCollection<IComparer<Subject>> aspects) =>
            Comparer<Subject>.Create(
                (a, b) =>
                {
                    foreach (var aspect in aspects)
                    {
                        if (aspect.Sign(a, b) is { } result and not Order.Sign.Equal)
                        {
                            return result.ToInt();
                        }
                    }

                    return 0;
                }
            );

        public IComparer<IEnumerable<Subject>> Sequence() => Sequence(Comparer<Subject>.Default);

        public IComparer<IEnumerable<Subject>> Sequence(IComparer<Subject> itemOrder) =>
            Comparer<IEnumerable<Subject>>.Create(
                (a, b) =>
                {
                    using var aEnumerator = a.GetEnumerator();
                    using var bEnumerator = b.GetEnumerator();
                    while (true)
                    {
                        var aMoved = aEnumerator.MoveNext();
                        var bMoved = bEnumerator.MoveNext();

                        switch (aMoved, bMoved)
                        {
                            case (false, false):
                                return Order.Sign.Equal.ToInt();

                            case (true, false):
                                return Order.Sign.Greater.ToInt();

                            case (false, true):
                                return Order.Sign.Less.ToInt();

                            case (true, true):
                                var comparison = itemOrder.Sign(aEnumerator.Current, bEnumerator.Current);
                                if (comparison != Order.Sign.Equal)
                                {
                                    return comparison.ToInt();
                                }

                                break;
                        }
                    }
                }
            );
    }
}