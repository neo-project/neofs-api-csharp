// Copyright (C) 2015-2025 The Neo Project.
//
// Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Netmap.Aggregator;
using Neo.FileStorage.API.Netmap.Normalize;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Netmap
{
    public static class Helper
    {
        public static Func<Node, double> WeightFunc(INormalizer c, INormalizer p)
        {
            return n =>
            {
                return c.Normalize(n.Capacity) * p.Normalize(n.Price);
            };
        }

        public static Func<Node, double> GenarateWeightFunc(this List<Node> ns)
        {
            var mean = new MeanAgg();
            var min = new MinAgg();
            for (int i = 0; i < ns.Count; i++)
            {
                mean.Add(ns[i].Capacity);
                min.Add(ns[i].Price);
            }
            return WeightFunc(new SigmoidNorm(mean.Compute()), new ReverseMinNorm(min.Compute()));
        }

        public static ulong Distance(this ulong x, ulong y)
        {
            var acc = x ^ y;
            acc ^= acc >> 33;
            acc *= 0xff51afd7ed558ccd;
            acc ^= acc >> 33;
            acc *= 0xc4ceb9fe1a85ec53;
            acc ^= acc >> 33;
            return acc;
        }

        public static List<Node> Flatten(this List<List<Node>> ns)
        {
            List<Node> nodes = new();
            foreach (var list in ns)
                foreach (var n in list)
                    nodes.Add(n);
            return nodes;
        }

        public static int GetBucketCount(this Selector selector)
        {
            if (selector.Clause == Clause.Same)
            {
                return 1;
            }
            return (int)selector.Count;
        }

        public static int GetNodesInBucket(this Selector selector)
        {
            if (selector.Clause == Clause.Same)
            {
                return (int)selector.Count;
            }
            return 1;
        }

        public static List<Node> InfoToNodes(this List<NodeInfo> nis)
        {
            return nis.Select((p, i) => new Node(i, p)).ToList();
        }
    }
}
