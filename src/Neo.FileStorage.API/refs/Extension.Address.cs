using System;
using Neo.IO.Json;

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
            return ContainerId.String() + "/" + ObjectId.String();
        }

        public static Address ParseString(string address)
        {
            var parts = address.Split('/');
            if (parts.Length != 2) throw new FormatException(nameof(ParseString) + " invalid address string");
            var cid = ContainerID.FromString(parts[0]);
            var oid = ObjectID.FromString(parts[1]);
            return new Address
            {
                ContainerId = cid,
                ObjectId = oid,
            };
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["containerID"] = ContainerId?.ToJson();
            json["objectID"] = ObjectId?.ToJson();
            return json;
        }
    }
}
