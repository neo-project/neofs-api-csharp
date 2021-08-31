#nullable enable
using System;

namespace Neo.FileStorage.API.Object
{
    public class ObjectException : Exception
    {
        public const string AlreadyRemovedError = "object already removed";
        public const string RangeOutOfBoundsError = "range out of bounds";
        public const string NotFoundError = "object not found";
        public const string SizeExceedLimitError = "object size exceed limit";
        public const string UnknownTypeError = "unknown object type";

        public ObjectException() : base() { }

        public ObjectException(string? message) : base(message) { }
    }

    public class SplitInfoException : ObjectException
    {
        public SplitInfo SplitInfo { get; private set; }

        public SplitInfoException(SplitInfo si)
        {
            SplitInfo = si;
        }
    }

    public class ObjectAlreadyRemovedException : ObjectException
    {
        public override string Message => AlreadyRemovedError;

        public ObjectAlreadyRemovedException() : base() { }
    }

    public class RangeOutOfBoundsException : ObjectException
    {
        public override string Message => RangeOutOfBoundsError;

        public RangeOutOfBoundsException() : base() { }
    }

    public class ObjectNotFoundException : ObjectException
    {
        public override string Message => NotFoundError;

        public ObjectNotFoundException() : base() { }
    }

    public class SizeExceedLimitException : ObjectException
    {
        public override string Message => SizeExceedLimitError;

        public SizeExceedLimitException() : base() { }
    }

    public class UnknownObjectTypeException : ObjectException
    {
        public override string Message => UnknownTypeError;

        public UnknownObjectTypeException() : base() { }
    }
}
