using System.Collections.Generic;
using Neo.FileStorage.API.Netmap;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    public static class Helper
    {
        public static Node GenerateTestNode(int index, params (string, string)[] attrs)
        {
            var ni = new NodeInfo();
            foreach (var item in attrs)
            {
                ni.Attributes.Add(new NodeInfo.Types.Attribute
                {
                    Key = item.Item1,
                    Value = item.Item2,
                });
            }
            return new Node(index, ni);
        }

        public static string NsIndexToString(this List<Node> ns)
        {
            string nss = "{";
            for (int j = 0; j < ns.Count; j++)
            {
                if (j != 0) nss += ", ";
                nss += ns[j].Index.ToString();
            }
            nss += "}";
            return nss;
        }
        public static string NodesIndexToString(this List<List<Node>> nodes)
        {
            string nss = "";
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i != 0) nss += ", ";
                nss += nodes[i].NsIndexToString();
            }
            return nss;
        }
    }
}