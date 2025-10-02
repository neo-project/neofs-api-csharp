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

using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Control
{
    public static class Helper
    {
        public static bool VerifyControlMessage(this IControlMessage message)
        {
            using var key = message.Signature.Key.ToByteArray().LoadPublicKey();
            return key.VerifyData(message.SignData.ToByteArray(), message.Signature.Sign.ToByteArray());
        }

        public static void SignControlMessage(this ECDsa key, IControlMessage message)
        {
            message.Signature = new()
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignData(message.SignData.ToByteArray())),
            };
        }
    }
}
