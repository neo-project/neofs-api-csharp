// Copyright (C) 2015-2025 The Neo Project.
//
// Encode.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Netmap;
using System.Collections.Generic;
using System.Text;

namespace Neo.FileStorage.API.Policy
{
    public static class Encode
    {
        public static string[] EncodePlacementPolicy(PlacementPolicy p)
        {
            if (p is null)
                return null;

            var replicas = new Replica[p.Replicas.Count];
            p.Replicas.CopyTo(replicas, 0);
            var selectors = new Selector[p.Selectors.Count];
            p.Selectors.CopyTo(selectors, 0);
            var filters = new Filter[p.Filters.Count];
            p.Filters.CopyTo(filters, 0);
            List<string> res = new();

            // print replicas
            res.AddRange(EncodeReplicas(replicas));
            // backup factor
            var cbf = p.ContainerBackupFactor;
            if (cbf != 0)
                res.Add("CBF " + cbf.ToString());
            // selectors
            res.AddRange(EncodeSelectors(selectors));
            // filters
            res.AddRange(EncodeFilters(filters));

            return res.ToArray();
        }

        public static string[] EncodeReplicas(Replica[] replicas)
        {
            if (replicas is null)
                return null;

            List<string> res = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var rep in replicas)
            {
                sb.Append("REP ");
                sb.Append(rep.Count);

                var s = rep.Selector;
                if (s != "")
                {
                    sb.Append(" IN ");
                    sb.Append(s);
                }

                res.Add(sb.ToString());
                sb.Clear();
            }

            return res.ToArray();
        }

        public static string[] EncodeSelectors(Selector[] selectors)
        {
            if (selectors is null)
                return null;

            List<string> res = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var sel in selectors)
            {
                sb.Append("SELECT ");
                sb.Append(sel.Count);

                if (sel.Attribute != "")
                {
                    sb.Append(" IN");
                    switch (sel.Clause)
                    {
                        case Clause.Same:
                            sb.Append(" SAME ");
                            break;
                        case Clause.Distinct:
                            sb.Append(" DISTINCT ");
                            break;
                        default:
                            sb.Append(" ");
                            break;
                    }
                    sb.Append(sel.Attribute);
                }

                if (sel.Filter != "")
                {
                    sb.Append(" FROM ");
                    sb.Append(sel.Filter);
                }

                if (sel.Name != "")
                {
                    sb.Append(" AS ");
                    sb.Append(sel.Name);
                }

                res.Add(sb.ToString());
                sb.Clear();
            }
            return res.ToArray();
        }

        public static string[] EncodeFilters(Filter[] filters)
        {
            if (filters is null)
                return null;

            List<string> res = new();
            StringBuilder sb = new();
            foreach (var filter in filters)
            {
                sb.Append("FILTER ");
                sb.Append(EncodeFilter(filter));

                res.Add(sb.ToString());
                sb.Clear();
            }

            return res.ToArray();
        }

        public static string EncodeFilter(Filter filter)
        {
            if (filter is null)
                return null;

            StringBuilder sb = new();
            var unspecified = filter.Op == Operation.Unspecified;

            if (filter.Key != "")
            {
                sb.Append(filter.Key);
                sb.Append(' ');
                sb.Append(filter.Op.AsString());
                sb.Append(' ');
                if (filter.Value.Contains(' '))
                    sb.Append(@"""" + filter.Value + @"""");
                else
                    sb.Append(filter.Value);
            }
            else if (unspecified && filter.Name != "")
            {
                sb.Append("@");
                sb.Append(filter.Name);
            }

            for (int i = 0; i < filter.Filters.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(" ");
                    sb.Append(filter.Op.AsString());
                    sb.Append(" ");
                }
                sb.Append(EncodeFilter(filter.Filters[i]));
            }

            if (filter.Name != "" && !unspecified)
            {
                sb.Append(" AS ");
                sb.Append(filter.Name);
            }

            return sb.ToString();
        }
    }
}
