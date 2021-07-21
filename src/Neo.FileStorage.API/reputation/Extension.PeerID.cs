using System;
using Google.Protobuf;
using Neo.Cryptography;

namespace Neo.FileStorage.API.Reputation
{
    public sealed partial class PeerID
    {
        public const int PeerIDLength = 33;

        public PeerID(byte[] bytes)
        {
            if (bytes is null || bytes.Length != PeerIDLength)
                throw new ArgumentException("invalid PeerID bytes", nameof(bytes));
            PublicKey = ByteString.CopyFrom(bytes);
        }

        public static PeerID Parse(string idString)
        {
            return new PeerID(Base58.Decode(idString));
        }

        public string String()
        {
            return Base58.Encode(PublicKey.ToByteArray());
        }

        public static implicit operator PeerID(byte[] value)
        {
            return new PeerID(value);
        }

        public static implicit operator byte[](PeerID pid)
        {
            return pid.PublicKey.ToByteArray();
        }
    }
}