using System;

namespace Neo.FileStorage.API.Object
{
    public class SplitInfoException : Exception
    {
        public SplitInfo SplitInfo { get; private set; }

        public SplitInfoException(SplitInfo si)
        {
            SplitInfo = si;
        }
    }

    public class ObjectAlreadyRemovedException : Exception
    {
        public override string Message => "object already removed";

        public ObjectAlreadyRemovedException() : base() { }
    }

    public class RangeOutOfBoundsException : Exception
    {
        public override string Message => "range out of bounds";

        public RangeOutOfBoundsException() : base() { }
    }

    public class ObjectNotFoundException : Exception
    {
        public override string Message => "object not found";

        public ObjectNotFoundException() : base() { }
    }

    public class ObjectSizeExceedLimitException : Exception
    {
        public override string Message => "object already removed";

        public ObjectSizeExceedLimitException() : base() { }
    }

    public class UnknownObjectTypeException : Exception
    {
        public override string Message => "unknown object type";

        public UnknownObjectTypeException() : base() { }
    }
}
