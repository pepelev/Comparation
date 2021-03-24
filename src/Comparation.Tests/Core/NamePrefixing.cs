namespace Comparation.Tests.Core
{
    public sealed class NamePrefixing : NamedTest
    {
        private readonly string prefix;
        private readonly NamedTest test;

        public NamePrefixing(string prefix, NamedTest test)
        {
            this.test = test;
            this.prefix = prefix;
        }

        public override string Name => $"{prefix}.{test.Name}";

        public override void Run()
        {
            test.Run();
        }
    }
}