namespace Comparation.Tests.Core
{
    public abstract class Named<T>
    {
        public abstract string Name { get; }
        public abstract T Value { get; }

        public static implicit operator Named<T>((string Name, T Value) tuple) =>
            new Plain(tuple.Name, tuple.Value);

        public override string ToString() => Name;

        public sealed class Plain : Named<T>
        {
            public Plain(string name, T value)
            {
                Name = name;
                Value = value;
            }

            public override string Name { get; }
            public override T Value { get; }
        }
    }
}