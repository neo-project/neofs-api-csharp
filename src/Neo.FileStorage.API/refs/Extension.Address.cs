using Neo.IO.Json;
using System;

namespace Neo.FileStorage.API.Refs
{
    public partial class Address
    {
        public Address(ContainerID cid, ObjectID oid)
        {
            ContainerId = cid;
            ObjectId = oid;
        }

        public string String()
        {
            return ContainerId.ToBase58String() + "/" + ObjectId.ToBase58String();
        }

        public static Address ParseString(string address)
        {
            var parts = address.Split('/');
            if (parts.Length != 2) throw new ArgumentException(nameof(ParseString) + " invalid address string");
            var cid = ContainerID.FromBase58String(parts[0]);
            var oid = ObjectID.FromBase58String(parts[1]);
            return new Address
            {
                ContainerId = cid,
                ObjectId = oid,
            };
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["objectid"] = ObjectId?.ToJson();
            json["containerid"] = ContainerId?.ToJson();
            return json;
        }
    }
}
