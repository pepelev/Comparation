using System;
using System.Collections.Generic;

namespace Comparation
{
    public static class Order
    {
        public static Order<T> Of<T>() => Order<T>.Singleton;
        public static Order<T> Of<T>(T sample) => Of<T>();
    }

    public sealed class Order<Subject>
    {
        internal static Order<Subject> Singleton { get; } = new();

        public IComparer<Subject> Trivial { get; } = Comparer<Subject>.Create(
            (_, _) => 0
        );

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
                        if (aspect.Compare(a, b) is { } result and not 0)
                        {
                            return result;
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

                        var comparison = (aMoved, bMoved) switch
                        {
                            (false, false) => 0,
                            (true, false) => 1,
                            (false, true) => -1,
                            (true, true) => itemOrder.Compare(aEnumerator.Current, bEnumerator.Current)
                        };

                        if (comparison is { } result and not 0)
                        {
                            return result;
                        }
                    }
                }
            );
    }
}