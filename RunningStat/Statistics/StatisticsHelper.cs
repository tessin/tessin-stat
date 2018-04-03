using System;
using System.Collections.Generic;

namespace Tessin.Statistics
{
    public static class StatisticsHelper
    {
        public static Mean Mean(this IEnumerable<double> source, double percentile = 1, int besselCorrection = 0)
        {
            if (!((0 <= percentile) & (percentile <= 1))) throw new ArgumentOutOfRangeException();

            var list = new List<double>();

            foreach (var x in source)
            {
                list.Add(x);
            }

            list.Sort((a, b) => a.CompareTo(b));

            double average; // median
            if ((list.Count & 1) == 0) // even
            {
                // 0, 1, 2, 3: 4
                var m = list.Count / 2;
                average = (list[m - 1] + list[m]) / 2;
            }
            else // odd
            {
                // 0, 1, 2: 3
                var m = list.Count / 2;
                average = (list[m]);
            }

            var min = Percentile(1 - percentile, list);
            var max = Percentile(percentile, list);

            var stdev = Stdev(list, average, besselCorrection);

            var dist = Distribution(list, average, stdev);

            return new Mean(average, min, max, stdev, dist);
        }

        /// <summary>
        /// Preferred estimation of percentiles according to NIST. Note that the list `sorted` must be sorted, ascending.
        /// </summary> 
        public static double Percentile(double percentile, IList<double> sorted)
        {
            // https://www.itl.nist.gov/div898/handbook/prc/section2/prc262.htm

            var c = sorted.Count;
            var n = (double)c;
            var np1 = (double)(c + 1);

            var min = 1d / np1;
            if (percentile <= min)
            {
                return sorted[0];
            }

            var max = n / np1;
            if (max <= percentile)
            {
                return sorted[sorted.Count - 1];
            }

            var i = percentile * np1;
            var k = Math.Floor(i);
            var d = i - k;
            var x = (int)k - 1; // zero based

            return sorted[x] + d * (sorted[x + 1] - sorted[x]);
        }

        /// <summary>
        /// Sample standard deviation, use `besselCorrection: 1` for population standard deviation.
        /// </summary>
        public static double Stdev(this IEnumerable<double> source, double average, int besselCorrection = 0)
        {
            var n = 0;
            var sum = 0.0;
            foreach (var x in source)
            {
                var y = x - average;
                sum += y * y;
                n++;
            }
            var stdev = besselCorrection < n ? Math.Sqrt(sum / (n - besselCorrection)) : double.NaN;
            return stdev;
        }

        /// <summary>
        /// Distribution of numbers within three standard deviations of the mean.
        /// </summary>
        public static double[] Distribution(this IEnumerable<double> source, double average, double stdev)
        {
            var lo0 = average - 1 * stdev;
            var hi0 = average + 1 * stdev;

            var lo1 = average - 2 * stdev;
            var hi1 = average + 2 * stdev;

            var lo2 = average - 3 * stdev;
            var hi2 = average + 3 * stdev;

            var a = 0;
            var b = 0;
            var c = 0;

            var n = 0;

            foreach (var x in source)
            {
                n++;
                if ((lo0 <= x) & (x <= hi0))
                {
                    a++;
                    continue;
                }
                if ((lo1 <= x) & (x <= hi1))
                {
                    b++;
                    continue;
                }
                if ((lo2 <= x) & (x <= hi2))
                {
                    c++;
                    continue;
                }
            }

            var m = (double)n;

            var dist = new[] { a / m, b / m, c / m };
            return dist;
        }
    }
}
