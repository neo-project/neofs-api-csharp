using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Context
    {
        public void ProcessSelectors(PlacementPolicy policy)
        {
            foreach (var selector in policy.Selectors)
            {
                if (selector is null)
                    throw new ArgumentException(nameof(ProcessSelectors) + " null selector in policy");
                if (selector.Filter != MainFilterName && !Filters.ContainsKey(selector.Filter))
                    throw new ArgumentException(nameof(ProcessSelectors) + " filter not found");
                Selectors[selector.Name] = selector;
                var results = GetSelection(policy, selector);
                Selections[selector.Name] = results;
            }
        }

        public List<List<Node>> GetSelection(PlacementPolicy policy, Selector sel)
        {
            int bucket_count = sel.GetBucketCount();
            int nodes_in_bucket = sel.GetNodesInBucket();
            var buckets = GetSelectionBase(sel).ToList();
            if (buckets.Count < bucket_count)
                throw new InvalidOperationException(nameof(GetSelection) + " not enough nodes");
            if (pivot is null)
            {
                if (sel.Attribute == "")
                    buckets.Sort((b1, b2) => b1.Item2[0].ID.CompareTo(b2.Item2[0].ID));
                else
                    buckets.Sort((b1, b2) => b1.Item1.CompareTo(b2.Item1));
            }
            var max_nodes_in_bucket = nodes_in_bucket * (int)Cbf;
            var nodes = new List<List<Node>>();
            var fallback = new List<List<Node>>();
            foreach (var (_, ns) in buckets)
            {
                if (max_nodes_in_bucket <= ns.Count)
                {
                    nodes.Add(ns.Take(max_nodes_in_bucket).ToList());
                }
                else if (nodes_in_bucket <= ns.Count)
                {
                    fallback.Add(ns);
                }
            }
            if (nodes.Count < bucket_count)
            {
                nodes = nodes.Concat(fallback).ToList();
                if (nodes.Count < bucket_count)
                    throw new InvalidOperationException(nameof(GetSelection) + " not enough nodes");
            }
            if (pivot is not null)
            {
                var list = nodes.Select((p, index) =>
                {
                    var agg = newAggregator();
                    foreach (var n in p)
                        agg.Add(weightFunc(n));
                    var w = agg.Compute();
                    var d = ((ulong)index).Distance(pivotHash);
                    return (d, w, p);
                }).ToList();
                var uniform = !list.Skip(1).Any(p => p.w != list[0].w);
                if (uniform)
                {
                    list.Sort((n1, n2) =>
                    {
                        return n1.d.CompareTo(n2.d);
                    });
                }
                else
                {
                    list.Sort((n1, n2) =>
                    {
                        var w1 = (~0u - n1.d) * n1.w;
                        var w2 = (~0u - n2.d) * n2.w;
                        return w2.CompareTo(w1);
                    });
                }
                nodes = list.Select(p => p.p).ToList();
            }
            if (sel.Attribute == "")
            {
                fallback = nodes.GetRange(bucket_count, nodes.Count - bucket_count);
                nodes = nodes.GetRange(0, bucket_count);
                for (int i = 0; i < fallback.Count; i++)
                {
                    var index = i % bucket_count;
                    if (nodes[index].Count >= max_nodes_in_bucket) break;
                    nodes[index] = nodes[index].Concat(fallback[i]).ToList();
                }
            }
            return nodes.GetRange(0, bucket_count);
        }

        public List<(string, List<Node>)> GetSelectionBase(Selector sel)
        {
            List<(string, List<Node>)> result = new();
            Filters.TryGetValue(sel.Filter, out Filter filter);
            if (sel.Attribute == "")
            {
                foreach (var node in Map.Nodes.Where(p => sel.Filter == MainFilterName || Match(filter, p)))
                    result.Add(("", new List<Node> { node }));
            }
            else
            {
                foreach (var group in Map.Nodes.Where(p => sel.Filter == MainFilterName || Match(filter, p)).GroupBy(p => p.Attributes[sel.Attribute]))
                {
                    result.Add((group.Key, group.ToList()));
                }
            }
            if (pivot is not null)
            {
                List<(string, List<Node>)> r = new();
                foreach (var group in result)
                {
                    var list = group.Item2.Select(p =>
                    {
                        p.Weight = weightFunc(p);
                        p.Distance = p.Hash.Distance(pivotHash);
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
                    r.Add((group.Item1, list));
                }
                result = r;
            }
            return result;
        }
    }
}
