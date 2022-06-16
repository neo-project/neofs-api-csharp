using System.Linq;

namespace Neo.FileStorage.API.Netmap
{
    public sealed partial class NodeInfo
    {
        public static partial class Types
        {
            public sealed partial class Attribute
            {
                private const string SysAttributePrefix = "__NEOFS__";
                public const string SysAttributeSubnet = SysAttributePrefix + "SUBNET_";

                public const string AttrSubnetValExit = "FALSE";
                public const string AttrSubnetValEntry = "TRUE";
            }
        }
    }
}
