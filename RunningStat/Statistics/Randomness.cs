using System;
using System.Collections.Generic;

namespace Tessin.Statistics
{
    public static class Randomness
    {
        public static Random NextRandom()
        {
            var seed = new byte[4];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(seed);
            }
            return new Random(BitConverter.ToInt32(seed, 0));
        }

        public static List<T> NextShuffle<T>(this Random r, IEnumerable<T> source)
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                var i = r.Next(list.Count + 1);
                if (i == list.Count)
                {
                    list.Add(item);
                }
                else
                {
                    var temp = list[i];
                    list[i] = item;
                    list.Add(temp);
                }
            }
            return list;
        }

        public static IEnumerable<int> NextSequence(this Random r, int maxValue)
        {
            for (; ; )
            {
                yield return r.Next(maxValue);
            }
        }
    }
}
