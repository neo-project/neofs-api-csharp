using Google.Protobuf;
using Neo.FileStorage.API.Cryptography.Tz;
using System;
using static Neo.FileStorage.API.Cryptography.Helper;

namespace Neo.FileStorage.API.Refs
{
    public sealed partial class Checksum
    {
        public Checksum(byte[] hash)
        {
            if (hash.Length == Sha256HashLength)
            {
                type_ = ChecksumType.Sha256;
                sum_ = ByteString.CopyFrom(hash);
            }
            else if (hash.Length == TzHash.TzHashLength)
            {
                type_ = ChecksumType.Tz;
                sum_ = ByteString.CopyFrom(hash);
            }
            else
            {
                throw new InvalidOperationException(nameof(Checksum) + " unsupported hash length");
            }

        }

        public bool Verify(ByteString data)
        {
            switch (type_)
            {
                case ChecksumType.Sha256:
                    {
                        return sum_ == data.Sha256();
                    }
                case ChecksumType.Tz:
                    {
                        return sum_ == data.TzHash();
                    }
                default:
                    throw new InvalidOperationException(nameof(Verify) + " unsupported checksum type " + type_);
            }
        }

        public string String()
        {
            return sum_.ToByteArray().ToHexString();
        }

        public void Parse(string str)
        {
            sum_ = ByteString.CopyFrom(str.HexToBytes());
            type_ = sum_.Length switch
            {
                Sha256HashLength => ChecksumType.Sha256,
                TzHash.TzHashLength => ChecksumType.Tz,
                _ => throw new FormatException($"unsupported checksum length {sum_.Length}"),
            };
        }
    }
}
