// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.MetaHeader.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public partial class RequestMetaHeader : IMetaHeader
    {
        public static RequestMetaHeader Default
        {
            get
            {
                var meta = new RequestMetaHeader()
                {
                    Version = Version.SDKVersion(),
                    Epoch = 0,
                    Ttl = 2,
                };
                return meta;
            }
        }

        public IMetaHeader GetOrigin()
        {
            return Origin;
        }
    }

    public partial class ResponseMetaHeader : IMetaHeader
    {
        public static ResponseMetaHeader Default
        {
            get
            {
                var meta = new ResponseMetaHeader()
                {
                    Version = new Version
                    {
                        Major = 2,
                        Minor = 0,
                    },
                    Epoch = 0,
                    Ttl = 1,
                };
                return meta;
            }
        }

        public IMetaHeader GetOrigin()
        {
            return Origin;
        }
    }
}
