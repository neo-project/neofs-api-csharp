
namespace Neo.FileStorage.API.Netmap.Aggregator
{
    public interface IAggregator
    {
        void Add(double w);
        double Compute();
        void Clear();
    }
}
