using Google.Protobuf;

namespace Neo.FileStorage.API.Control
{
    public partial class HealthCheckRequest : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class HealthCheckResponse : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class NetmapSnapshotRequest : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class NetmapSnapshotResponse : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class SetNetmapStatusRequest : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class SetNetmapStatusResponse : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class DropObjectsRequest : IControlMessage
    {
        public IMessage SignData => Body;
    }

    public partial class DropObjectsResponse : IControlMessage
    {
        public IMessage SignData => Body;
    }
}
