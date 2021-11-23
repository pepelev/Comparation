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

            public bool Equals(Box<T?> x, Box<T?> y) => (x.value, y.value) switch
            {
                ({ } a, { } b) => equality.Equals(a, b),
                (null, null) => true,
                _ => false
            };

            public int GetHashCode(Box<T?> obj) => obj.value is { } value
                ? equality.GetHashCode(value)
                : 0;
        }
    }
}