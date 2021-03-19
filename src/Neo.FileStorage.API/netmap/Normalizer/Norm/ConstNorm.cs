

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class ConstNorm : INormalizer
    {
        private readonly double value;

        public ConstNorm(double value)
        {
            this.value = value;
        }

        public double Normalize(double w)
        {
            return value;
        }
    }
}
