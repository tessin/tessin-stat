using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tessin.Statistics;

namespace Tessin
{
    public static class X
    {
        public enum Type
        {
            X,
            Y,
            Z
        }

        public static void AddX(this RunningStatValueBucket b, double v)
        {
            b.Add((int)Type.X, v);
        }

        public static void AddY(this RunningStatValueBucket b, double v)
        {
            b.Add((int)Type.Y, v);
        }

        public static void AddZ(this RunningStatValueBucket b, double v)
        {
            b.Add((int)Type.Z, v);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mgr = RunningStatValueBucketManager.Create<X.Type>(TimeSpan.FromSeconds(15));

            var b = mgr.GetBucket(DateTime.UtcNow);
            b.AddX(2);

            var b2 = mgr.GetBucket(DateTime.UtcNow.AddSeconds(15));
            b2.AddY(3);

            var b3 = mgr.GetBuckets(DateTime.UtcNow.AddSeconds(30));
        }
    }
}
