// Copyright (C) 2015-2025 The Neo Project.
//
// EAclWithSignature.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public class EAclWithSignature
    {
        public EACLTable Table;
        public SignatureRFC6979 Signature;
        public SessionToken SessionToken;
    }
}
