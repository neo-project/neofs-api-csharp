using System;

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public interface INormalizer
    {
        double Normalize(double w);
    }
}
