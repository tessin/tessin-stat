using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tessin.Statistics
{
    public class RunningStatValueBucket
    {
        public static RunningStatValueBucket GetBucket(ref RunningStatValueBucket _bucket, long k, int n)
        {
            int m = 0;
            _GetBucket:
            var bucket = _bucket;
            if (bucket == null)
            {
                var bucket2 = new RunningStatValueBucket(null, k, n);
                if (Interlocked.CompareExchange(ref _bucket, bucket2, null) != null)
                {
                    if (m < 1)
                    {
                        m++;
                        goto _GetBucket;
                    }
                    return bucket2; // discard
                }
                return bucket2;
            }
            if (bucket._k < k)
            {
                var bucket3 = new RunningStatValueBucket(bucket, k, n);
                if (Interlocked.CompareExchange(ref _bucket, bucket3, bucket) != bucket)
                {
                    if (m < 1)
                    {
                        m++;
                        goto _GetBucket;
                    }
                    return bucket3; // discard
                }
                return bucket3;
            }
            return bucket;
        }

        public static List<RunningStatValueBucket> GetBuckets(ref RunningStatValueBucket _bucket, long k)
        {
            var bucket = _bucket;
            if (bucket == null)
            {
                return null;
            }

            var list = new List<RunningStatValueBucket>();

            while (bucket != null)
            {
                if (bucket._k < k)
                {
                    list.Add(bucket);
                }
                var bucket2 = bucket;
                bucket = bucket2._prev;
                bucket2._prev = null;
            }

            list.Reverse();

            return list;
        }

        private RunningStatValueBucket _prev;

        private readonly long _k;
        private readonly RunningStatValue[] _stat;
        private readonly double[] _n;

        public RunningStatValueBucket(RunningStatValueBucket prev, long k, int n)
        {
            _prev = prev;
            _k = k;
            _stat = new RunningStatValue[n];
            _n = new double[n];
        }

        public void Add(int index, double v)
        {
            if (index < _stat.Length)
            {
                RunningStatValue.Add(ref _stat[index], ref _n[index], v);
            }
        }
    }
}
