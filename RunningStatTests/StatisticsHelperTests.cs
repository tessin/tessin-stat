using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tessin.Statistics;

namespace Tessin
{
    [TestClass]
    public class StatisticsHelperTests
    {
        [TestMethod]
        [DataRow(1, new double[] { 1 })]
        [DataRow(1.5, new double[] { 1, 2 })]
        [DataRow(2, new double[] { 1, 2, 3 })]
        [DataRow(3, new double[] { 2, 3, 5 })]
        [DataRow(4, new double[] { 2, 3, 5, 7 })]
        public void StatisticsHelper_Mean_Average_Test(double expected, double[] xs)
        {
            var r = Randomness.NextRandom();

            for (int i = 0; i < 10; i++)
            {
                var mean = r.NextShuffle(xs).Mean();
                Assert.AreEqual(expected, mean.Average);
            }
        }

        [TestMethod]
        [DataRow(7, 0, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 0)] // 0th percentile (min/max swap)
        [DataRow(6.875, 0.125, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 1.0 / 8)] // 1/8th percentile
        [DataRow(3.5, 3.5, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 0.5)] // mean
        [DataRow(1.8125, 5.1875, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 11.0 / 16)] // 11/16th percentile
        [DataRow(1.25, 5.75, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 6.0 / 8)] // 6/8th percentile
        [DataRow(0.6875, 6.3125, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 13.0 / 16)] // 13/16th percentile
        [DataRow(0, 7, new double[] { 0, 1, 2, 3, 4, 5, 6, 7 }, 1)]
        public void StatisticsHelper_Mean_Percentile_Test(double min, double max, double[] xs, double percentile)
        {
            var mean = xs.Mean(percentile);

            Assert.AreEqual(min, mean.Min);
            Assert.AreEqual(max, mean.Max);
        }
    }
}
