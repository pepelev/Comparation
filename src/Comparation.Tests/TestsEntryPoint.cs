using Comparation.Tests.Core;
using NUnit.Framework;

namespace Comparation.Tests
{
    public sealed class TestsEntryPoint
    {
        [Test]
        [TestCaseSource(typeof(Equality.Cases))]
        [TestCaseSource(typeof(Order.Cases))]
        public void Run(Test test)
        {
            test.Run();
        }
    }
}