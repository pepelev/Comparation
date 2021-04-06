using System.Collections.Generic;

namespace Comparation
{
    internal readonly struct Value<T>
    {
        private readonly T value;

        public Value(T value)
        {
            this.value = value;
        }

        public override string? ToString() => value is { } content
            ? content.ToString()
            : "null";

        public sealed class Equality : IEqualityComparer<Value<T?>>
        {
            private readonly IEqualityComparer<T> equality;

            public Equality(IEqualityComparer<T> equality)
            {
                this.equality = equality;
            }

            public bool Equals(Value<T?> x, Value<T?> y) => (x.value, y.value) switch
            {
                ({ } a, { } b) => equality.Equals(a, b),
                (null, null) => true,
                _ => false
            };

            public int GetHashCode(Value<T?> obj) => obj.value is { } value
                ? equality.GetHashCode(value)
                : 0;
        }
    }
}