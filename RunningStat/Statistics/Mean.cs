using System;

namespace Tessin.Statistics
{
    public struct Mean
    {
        /// <summary>
        /// Median value separating the higher half of the data sample from the lower half. aka the "middle" value. The median is a robust measure of central tendency.
        /// </summary>
        public readonly double Average;

        /// <summary>
        /// Min/max percentiles as using the preferred method according to NIST, https://www.itl.nist.gov/div898/handbook/prc/section2/prc262.htm
        /// </summary>
        public readonly double Min, Max;

        /// <summary>
        /// Sample standard deviation (unless Bessel's correction was used, Stdev(..., besselCorrection: 1)). 
        /// </summary>
        public readonly double Stdev;

        /// <summary>
        /// Distribution of values according to the three-sigma rule.
        /// </summary>
        public readonly double[] Dist;

        public Mean(double average, double min, double max, double stdev, double[] dist)
        {
            this.Average = average;
            this.Min = min;
            this.Max = max;
            this.Stdev = stdev;
            this.Dist = dist;
        }

        public override string ToString()
        {
            return FormattableString.Invariant($"Avg={Average:0.000}, Min={Min:0.000}, Max={Max:0.000}, Stdev={Stdev:0.000}");
        }
    }
}
