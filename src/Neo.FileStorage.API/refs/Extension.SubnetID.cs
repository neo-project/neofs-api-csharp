using Org.BouncyCastle.Crypto.Paddings;

namespace Neo.FileStorage.API.Refs
{
    public sealed partial class SubnetID
    {
        public static SubnetID Zero => new() { Value = 0 };
    }
}
