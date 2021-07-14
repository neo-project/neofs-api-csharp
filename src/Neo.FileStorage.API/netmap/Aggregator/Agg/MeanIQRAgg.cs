using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Netmap.Aggregator
{
    public class MeanIQRAgg : IAggregator
    {
        private readonly double k;
        private List<double> arr = new();

        public MeanIQRAgg(double k)
        {
            this.k = k;
        }

        public void Add(double n)
        {
            arr.Add(n);
        }

        public double Compute()
        {
            if (arr.Count == 0)
                return 0;

            arr.Sort();

            double min, max;
            if (arr.Count < 4)
            {
                max = arr[^1];
                min = arr[0];
            }
            else
            {
                var start = arr.Count / 4;
                var end = arr.Count * 3 / 4 - 1;
                var iqr = k * (arr[start] - arr[end]);
                max = arr[end] + iqr;
                min = arr[start] - iqr;
            }

            int count = 0;
            double sum = 0;

            foreach (var e in arr)
            {
                if (min <= e && e <= max)
                {
                    sum += e;
                    count++;
                }
            }

            return sum / count;
        }

        public void Clear()
        {
            arr = new();
        }
    }
}
