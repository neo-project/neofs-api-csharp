// Copyright (C) 2015-2025 The Neo Project.
//
// NetMap.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Netmap
{
    public class NetMap
    {
        public List<Node> Nodes;

        public NetMap(List<Node> ns)
        {
            if (ns is null) return;
            Nodes = ns;
        }

        public List<List<Node>> GetPlacementVectors(List<List<Node>> ns, byte[] pivot)
        {
            if (pivot is null) return ns;
            var h = pivot.Murmur64(0);
            var weightFunc = Nodes.GenarateWeightFunc();
            var results = new List<List<Node>>();
            ns.ForEach(ns =>
            {
                var list = ns.Select(p =>
                {
                    p.Weight = weightFunc(p);
                    p.Distance = p.Hash.Distance(h);
                    return p;
                }).ToList();
                var uniform = !list.Skip(1).Any(p => p.Weight != list[0].Weight);
                if (uniform)
                {
                    list.Sort((n1, n2) =>
                    {
                        return n1.Distance.CompareTo(n2.Distance);
                    });
                }
                else
                {
                    list.Sort((n1, n2) =>
                    {
                        var w1 = (~0u - n1.Distance) * n1.Weight;
                        var w2 = (~0u - n2.Distance) * n2.Weight;
                        return w2.CompareTo(w1);
                    });
                }
                results.Add(list);
            });
            return results;
        }

        public List<List<Node>> GetContainerNodes(PlacementPolicy policy, byte[] pivot)
        {
            if (policy is null) throw new ArgumentNullException(nameof(policy));
            var context = new Context(this);
            context.SetPivot(pivot);
            context.SetCBF(policy.ContainerBackupFactor);
            context.ProcessFilters(policy);
            context.ProcessSelectors(policy);
            var result = new List<List<Node>>();
            foreach (var replica in policy.Replicas)
            {
                if (replica is null)
                    throw new ArgumentException(nameof(GetContainerNodes) + " missing Replicas");
                var r = new List<Node>();
                if (replica.Selector == "")
                {
                    if (policy.Selectors.Count == 0)
                    {
                        var s = new Selector
                        {
                            Count = replica.Count,
                            Filter = Context.MainFilterName,
                        };
                        var ns = context.GetSelection(policy, s);
                        r = ns.Flatten().ToList();
                    }
                    foreach (var selector in policy.Selectors)
                        r = r.Concat(context.Selections[selector.Name].Flatten()).ToList();
                    result.Add(r);
                    continue;
                }
                var nodes = context.Selections[replica.Selector];
                r = nodes.Flatten().ToList();
                result.Add(r);
            }
            return result;
        }
    }
}
