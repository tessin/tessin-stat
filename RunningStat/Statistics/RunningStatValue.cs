using System;

// https://www.johndcook.com/blog/standard_deviation/

namespace Tessin.Statistics
{
    public struct RunningStatValue
    {
        public static void Add(ref RunningStatValue stat, ref double n, double x)
        {
            var np1 = n + 1;

            var oldM = stat._oldM;
            var oldS = stat._oldS;
            var newM = oldM + (x - oldM) / np1;
            var newS = oldS + (x - oldM) * (x - newM);

            stat._oldM = newM;
            stat._oldS = newS;
            stat._newM = newM;
            stat._newS = newS;

            n = np1;
        }

        private double _oldM, _oldS, _newM, _newS;
    }
}
