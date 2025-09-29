using Comparation.Tests.Core;
using NUnit.Framework;

[assembly: LevelOfParallelism(8)]

namespace Comparation.Tests
{
    [Parallelizable(ParallelScope.All)]
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