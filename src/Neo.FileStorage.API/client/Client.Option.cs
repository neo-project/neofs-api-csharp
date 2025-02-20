// Copyright (C) 2015-2025 The Neo Project.
//
// Client.Option.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Session;
using System;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        private Action<IResponse> responseHandler;

        public Client WithResponseInfoHandler(Action<IResponse> handler)
        {
            responseHandler = handler;
            return this;
        }
    }
}
