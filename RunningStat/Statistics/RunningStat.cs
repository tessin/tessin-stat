using System;

// https://www.johndcook.com/blog/standard_deviation/

namespace Tessin.Statistics
{
    public class RunningStat
    {
        private double m_oldM, m_newM, m_oldS, m_newS;

        private int m_n;

        /// <summary>
        /// Sample size
        /// </summary>
        public int SampleSize => m_n;

        public double Average => 0 < m_n ? m_newM : double.NaN;
        public double Variance => 1 < m_n ? m_newS / (m_n - 1) : double.NaN;
        public double Stdev => Math.Sqrt(Variance);

        private int _bucket1;
        private int _bucket2;
        private int _bucket3;

        public double[] Distribution => new double[] { (double)_bucket1 / m_n, (double)_bucket2 / m_n, (double)_bucket3 / m_n };

        public void Reset()
        {
            this.m_oldM = 0;
            this.m_oldS = 0;
            this.m_newM = 0;
            this.m_newS = 0;

            this.m_n = 0;

            this._bucket1 = 0;
            this._bucket2 = 0;
            this._bucket3 = 0;
        }

        public void Push(double x)
        {
            var n = this.m_n + 1;

            var m = 1d / n;

            var oldM = this.m_oldM;
            var oldS = this.m_oldS;
            var newM = oldM + m * (x - oldM);
            var newS = oldS + (x - oldM) * (x - newM);

            // set up for next iteration
            this.m_oldM = newM;
            this.m_oldS = newS;
            this.m_newM = newM;
            this.m_newS = newS;

            this.m_n = n;

            if (1 < n)
            {
                // approximate probability distribution

                var avg = Average;
                var stdev = Stdev;

                var y = Math.Abs(x - avg);
                if (y <= stdev)
                {
                    _bucket1++;
                    return;
                }
                if (y <= 2 * stdev)
                {
                    _bucket2++;
                    return;
                }
                if (y <= 3 * stdev)
                {
                    _bucket3++;
                    return;
                }
            }
        }

        public override string ToString()
        {
            return FormattableString.Invariant($"N={SampleSize}, Average={Average:0.000}, Stdev={Stdev:0.000}");
        }
    }
}
