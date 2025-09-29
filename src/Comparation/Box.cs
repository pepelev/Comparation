using System.Collections.Generic;

namespace Comparation
{
    internal readonly struct Box<T>
    {
        private readonly T value;

        public Box(T value)
        {
            this.value = value;
        }

        public override string? ToString() => value is { } content
            ? content.ToString()
            : "null";

        public sealed class Equality : IEqualityComparer<Box<T?>>
        {
            private readonly IEqualityComparer<T> equality;

            public Equality(IEqualityComparer<T> equality)
            {
                this.equality = equality;
            }

#if NETSTANDARD2_0 || NETSTANDARD2_1
            public bool Equals(Box<T?> x, Box<T?> y) => equality.Equals(x.value!, y.value!);
#else
            public bool Equals(Box<T?> x, Box<T?> y) => equality.Equals(x.value, y.value);
#endif

            public int GetHashCode(Box<T?> obj) => obj.value is { } value
                ? equality.GetHashCode(value)
                : 0;
        }
    }

    internal static class Box
    {
        public static Box<T> Wrap<T>(T item) => new(item);
    }
}