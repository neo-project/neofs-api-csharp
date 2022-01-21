using System;
using System.Collections.Generic;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap.Aggregator;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Context
    {
        public const string ErrMissingField = "netmap: nil field";
        public const string ErrInvalidFilterName = "netmap: filter name is invalid";
        public const string ErrInvalidNumber = "netmap: number value expected";
        public const string ErrInvalidFilterOp = "netmap: invalid filter operation";
        public const string ErrFilterNotFound = "netmap: filter not found";
        public const string ErrNonEmptyFilters = "netmap: simple filter must no contain sub-filters";
        public const string ErrNotEnoughNodes = "netmap: not enough nodes to SELECT from";
        public const string ErrSelectorNotFound = "netmap: selector not found";
        public const string ErrUnnamedTopFilter = "netmap: all filters on top level must be named";

        public const uint DefaultCBF = 3;
        public NetMap Map;
        public Dictionary<string, Filter> Filters = new();
        public Dictionary<string, Selector> Selectors = new();
        public Dictionary<string, List<List<Node>>> Selections = new();
        public Dictionary<Filter, ulong> NumCache = new();
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
                Cbf = DefaultCBF;
            else
                Cbf = cbf;
        }
    }
}
