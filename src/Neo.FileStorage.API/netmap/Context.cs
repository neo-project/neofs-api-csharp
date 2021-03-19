using Neo.FileStorage.API.Netmap.Aggregator;
using Neo.FileStorage.API.Cryptography;
using System;
using System.Collections.Generic;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Context
    {
        public const uint DefaultCBF = 3;
        public NetMap Map;
        public Dictionary<string, Filter> Filters = new();
        public Dictionary<string, Selector> Selectors = new();
        public Dictionary<string, List<List<Node>>> Selections = new();
        public Dictionary<Filter, UInt64> NumCache = new();
        private byte[] pivot;
        private ulong pivotHash;
        private Func<IAggregator> newAggregator;
        private Func<Node, double> weightFunc;
        public uint Cbf { get; private set; }

        public Context(NetMap map)
        {
            Map = map;
            newAggregator = () => new MeanIQRAgg(0);
            weightFunc = map.Nodes.GenarateWeightFunc();
            Cbf = DefaultCBF;
        }

        public void SetPivot(byte[] pivot)
        {
            if (pivot is null || pivot.Length == 0) return;
            this.pivot = pivot;
            pivotHash = pivot.Murmur64(0);
        }

        public void SetWeightFunc(Func<Node, double> f)
        {
            weightFunc = f;
        }

        public void SetAggregator(Func<IAggregator> agg)
        {
            newAggregator = agg;
        }

        public void SetCBF(uint cbf)
        {
            if (cbf == 0)
                this.Cbf = DefaultCBF;
            else
                this.Cbf = cbf;
        }
    }
}
