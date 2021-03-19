

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class MaxNorm : INormalizer
    {
        private readonly double max;

        public MaxNorm(double max)
        {
            this.max = max;
        }

        public double Normalize(double w)
        {
            if (max == 0) return 0;
            return w / max;
        }
    }
}
