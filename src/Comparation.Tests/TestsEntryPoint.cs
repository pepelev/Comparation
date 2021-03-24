using Comparation.Tests.Core;
using Comparation.Tests.Equality;
using NUnit.Framework;

namespace Comparation.Tests
{
    public sealed class TestsEntryPoint
    {
        [Test]
        [TestCaseSource(typeof(Cases))]
        public void Run(Test test)
        {
            test.Run();
        }
    }
}