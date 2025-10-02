// Copyright (C) 2015-2025 The Neo Project.
//
// Grammer.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Sprache;
using System;

namespace Neo.FileStorage.API.Policy
{
    public class Query
    {
        public ReplicaStmt[] Replicas { get; set; }
        public uint CBF { get; set; }
        public SelectorStmt[] Selectors { get; set; }
        public FilterStmt[] Filters { get; set; }

        public Query(ReplicaStmt[] replicas, uint cbf, SelectorStmt[] selectors, FilterStmt[] filters)
        {
            Replicas = replicas;
            CBF = cbf;
            Selectors = selectors;
            Filters = filters;
        }

        public static Query Parse(string s)
        {
            return Helper.QueryParser.Parse(s);
        }
    }

    public class ReplicaStmt
    {
        public uint Count { get; set; }
        public string Selector { get; set; }

        public ReplicaStmt(uint count, string selector)
        {
            Count = count;
            Selector = selector;
        }
    }

    public class SelectorStmt
    {
        public uint Count { get; set; }
        public string[] Bucket { get; set; }
        public string Filter { get; set; }
        public string Name { get; set; }

        public SelectorStmt(uint count, string[] bucket, string filter, string name)
        {
            Count = count;
            Bucket = bucket;
            Filter = filter;
            Name = name;
        }
    }

    public class FilterStmt
    {
        public OrChain Value { get; set; }
        public string Name { get; set; }

        public FilterStmt(OrChain value, string name)
        {
            Value = value;
            Name = name;
        }
    }

    public class OrChain
    {
        public AndChain[] Clauses { get; set; }

        public OrChain(AndChain[] clauses)
        {
            Clauses = clauses;
        }
    }

    public class AndChain
    {
        public FilterOrExpr[] Clauses { get; set; }

        public AndChain(FilterOrExpr[] clauses)
        {
            Clauses = clauses;
        }
    }

    public class FilterOrExpr
    {
        public string Reference { get; set; }
        public SimpleExpr Expr { get; set; }

        public FilterOrExpr(string value)
        {
            if (value.StartsWith("@"))
            {
                Reference = value[1..];
                Expr = null;
            }
            else
            {
                var values = value.Split(Helper.ExprSpliter);
                if (values.Length != 3)
                    throw new ArgumentException($"invalid filter expression {value}");
                Expr = new SimpleExpr(values[0], values[1], values[2]);
                Reference = "";
            }
        }
    }

    public class SimpleExpr
    {
        public string Key { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }

        public SimpleExpr(string key, string op, string value)
        {
            Key = key;
            Op = op;
            Value = value;
        }
    }
}
