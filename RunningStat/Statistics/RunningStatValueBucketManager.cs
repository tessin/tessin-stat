using System;
using System.Collections.Generic;
using System.Linq;

namespace Tessin.Statistics
{
    public class RunningStatValueBucketManager
    {
        public static RunningStatValueBucketManager Create<TEnum>(TimeSpan window)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return new RunningStatValueBucketManager(window, Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Max().ToInt32(null) + 1);
        }

        private readonly TimeSpan _window;
        private readonly int _n;
        private RunningStatValueBucket _bucket;

        public RunningStatValueBucketManager(TimeSpan window, int n)
        {
            _window = window;
            _n = n;
        }

        public RunningStatValueBucket GetBucket(DateTime? utcNow = null)
        {
            return RunningStatValueBucket.GetBucket(ref _bucket, (utcNow ?? DateTime.UtcNow).Ticks / _window.Ticks, _n);
        }

        public List<RunningStatValueBucket> GetBuckets(DateTime? utcNow = null)
        {
            return RunningStatValueBucket.GetBuckets(ref _bucket, (utcNow ?? DateTime.UtcNow).Ticks / _window.Ticks);
        }
    }
}
