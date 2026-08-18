// Copyright (C) 2015-2025 The Neo Project.
//
// SearchFilters.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.Object
{
    public class SearchFilters
    {
        private readonly List<SearchFilter> filters = new();

        public SearchFilter[] Filters => filters.ToArray();

        public SearchFilters() { }

        public SearchFilters(IEnumerable<SearchFilter> fs)
        {
            filters = fs.ToList();
        }

        public void AddFilter(string name, string value, MatchType op)
        {
            filters.Add(new SearchFilter
            {
                Key = name,
                Value = value,
                MatchType = op,
            });
        }

        public void AddObjectVersionFilter(MatchType op, Version v)
        {
            AddFilter(SearchFilter.FilterHeaderVersion, v.String(), op);
        }

        public void AddObjectContainerIDFilter(MatchType op, ContainerID cid)
        {
            AddFilter(SearchFilter.FilterHeaderContainerID, cid.String(), op);
        }

        public void AddObjectOwnerIDFilter(MatchType op, OwnerID oid)
        {
            AddFilter(SearchFilter.FilterHeaderOwnerID, oid.ToAddress(), op);
        }

        public void AddRootFilter()
        {
            AddFilter(SearchFilter.FilterPropertyRoot, "", MatchType.Unspecified);
        }

        public void AddPhyFilter()
        {
            AddFilter(SearchFilter.FilterPropertyPhy, "", MatchType.Unspecified);
        }

        public void AddParentIDFilter(MatchType op, ObjectID oid)
        {
            AddFilter(SearchFilter.FilterHeaderParent, oid.String(), op);
        }

        public void AddObjectIDFilter(MatchType op, ObjectID oid)
        {
            AddFilter(SearchFilter.FilterHeaderObjectID, oid.String(), op);
        }

        public void AddSplitIDFilter(MatchType op, SplitID sid)
        {
            AddFilter(SearchFilter.FilterHeaderSplitID, sid.ToString(), op);
        }

        public void AddTypeFilter(MatchType op, ObjectType typ)
        {
            AddFilter(SearchFilter.FilterHeaderObjectType, typ.String(), op);
        }
    }
}
