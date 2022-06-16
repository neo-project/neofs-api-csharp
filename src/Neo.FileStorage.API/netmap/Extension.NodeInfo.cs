using Google.Protobuf;
using Neo.FileStorage.API.Refs;
using System.Collections.Generic;
using System.Linq;
using static Neo.FileStorage.API.Netmap.NodeInfo.Types;

namespace Neo.FileStorage.API.Netmap
{
    public sealed partial class NodeInfo
    {
        public static string SubnetAttributeKey(SubnetID id)
        {
            return Attribute.SysAttributeSubnet + id.Value;
        }

        public List<SubnetID> Subnets
        {
            get
            {
                List<SubnetID> subnets = new();
                foreach (var attr in Attributes)
                {
                    if (attr.Key.Length > Attribute.SysAttributeSubnet.Length
                        && attr.Key.StartsWith(Attribute.SysAttributeSubnet)
                        && uint.TryParse(attr.Key[Attribute.SysAttributeSubnet.Length..], out uint id))
                    {
                        SubnetID subnet = new() { Value = id };
                        if (attr.Value == Attribute.AttrSubnetValEntry)
                        {
                            if (!subnets.Contains(subnet))
                                subnets.Add(new() { Value = id });
                        }
                        else if (id == 0)
                        {
                            attr.Value = Attribute.AttrSubnetValEntry;
                            subnets.Add(new() { Value = id });
                        }
                    }
                }
                if (!subnets.Contains(SubnetID.Zero))
                {
                    attributes_.Add(new Attribute { Key = SubnetAttributeKey(SubnetID.Zero), Value = Attribute.AttrSubnetValEntry });
                    subnets.Add(SubnetID.Zero);
                }
                return subnets;
            }
        }
    }
}
