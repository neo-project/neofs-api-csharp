using System;
using Google.Protobuf;

namespace Neo.FileStorage.API.Object
{
    public class SplitID : IComparable<SplitID>, IEquatable<SplitID>
    {
        public const int Size = 16;

        private Guid guid;

        public SplitID()
        {
            guid = Guid.NewGuid();
        }

        public bool Parse(string str)
        {
            return Guid.TryParse(str, out guid);
        }

        public override string ToString()
        {
            return guid == Guid.Empty ? "" : guid.ToString();
        }

        public void SetGuid(Guid g)
        {
            if (g != Guid.Empty)
                guid = g;
        }

        public byte[] ToByteArray()
        {
            return guid.ToByteArray();
        }

        public ByteString ToByteString()
        {
            return ByteString.CopyFrom(ToByteArray());
        }

        public bool Equals(SplitID other)
        {
            if (guid == Guid.Empty || other.guid == Guid.Empty)
                return false;
            return ToString() == other.ToString();
        }

        public int CompareTo(SplitID other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public static implicit operator SplitID(byte[] bytes)
        {
            if (bytes is null) return null;
            return new()
            {
                guid = new(bytes)
            };
        }

        public static implicit operator SplitID(ByteString bytes)
        {
            if (bytes is null) return null;
            return new()
            {
                guid = new(bytes.ToByteArray())
            };
        }

        public static implicit operator byte[](SplitID s)
        {
            return s?.ToByteArray();
        }

        public static implicit operator ByteString(SplitID s)
        {
            return s?.ToByteString();
        }
    }
}
