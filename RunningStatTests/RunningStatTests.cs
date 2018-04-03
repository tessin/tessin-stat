using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tessin.Statistics;

namespace Tessin
{
    [TestClass]
    public class RunningStatTests
    {
        [TestMethod]
        public void RunningStat_Average_Test1()
        {
            foreach (var n in new[] { 1, 2, 3, 10, 100, 1000 })
            {
                var xs = Randomness.NextRandom().NextSequence(n).Select(x => (double)x).Take(n).ToList();
                var stat = new RunningStat();
                foreach (var x in xs)
                {
                    stat.Push(x);
                }
                Assert.AreEqual(xs.Average(), stat.Average, 1d / 1000000);
            }
        }

        [TestMethod]
        public void RunningStat_Stdev_Test1()
        {
            foreach (var n in new[] { 10, 100, 1000 })
            {
                var xs = Randomness.NextRandom().NextSequence(n).Select(x => (double)x).Take(n).ToList();
                var stat = new RunningStat();
                foreach (var x in xs)
                {
                    stat.Push(x);
                }
                var stdev = xs.Stdev(xs.Average());
                var error = stdev - stat.Stdev;
                Assert.AreEqual(stdev, stat.Stdev, 0.25, $"e={error}");
            }
        }

        [TestMethod]
        public void RunningStat_Stdev_Test2()
        {
            var stat = new RunningStat();

            stat.Push(0);
            stat.Push(0);
            stat.Push(0);

            //Assert.AreEqual()
        }
    }
}
