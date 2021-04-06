namespace Comparation.Tests.Core
{
    public sealed class NamePrefixing<T> : Named<T>
    {
        private readonly string prefix;
        private readonly Named<T> named;

        public NamePrefixing(string prefix, Named<T> named)
        {
            this.prefix = prefix;
            this.named = named;
        }

        public override string Name => $"{prefix}.{named.Name}";
        public override T Value => named.Value;
    }
}