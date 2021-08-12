#nullable enable
using System;

namespace Neo.FileStorage.API.Object
{
    public class ObjectExcpetion : Exception
    {
        public const string AlreadyRemovedError = "object already removed";
        public const string RangeOutOfBoundsError = "range out of bounds";
        public const string NotFoundError = "object not found";
        public const string SizeExceedLimitError = "object size exceed limit";
        public const string UnknownTypeError = "unknown object type";

        public ObjectExcpetion() : base() { }

        public ObjectExcpetion(string? message) : base(message) { }
    }

    public class SplitInfoException : ObjectExcpetion
    {
        public SplitInfo SplitInfo { get; private set; }

        public SplitInfoException(SplitInfo si)
        {
            SplitInfo = si;
        }
    }

    public class ObjectAlreadyRemovedException : ObjectExcpetion
    {
        public override string Message => AlreadyRemovedError;

        public ObjectAlreadyRemovedException() : base() { }
    }

    public class RangeOutOfBoundsException : ObjectExcpetion
    {
        public override string Message => RangeOutOfBoundsError;

        public RangeOutOfBoundsException() : base() { }
    }

    public class ObjectNotFoundException : ObjectExcpetion
    {
        public override string Message => NotFoundError;

        public ObjectNotFoundException() : base() { }
    }

    public class SizeExceedLimitException : ObjectExcpetion
    {
        public override string Message => SizeExceedLimitError;

        public SizeExceedLimitException() : base() { }
    }

    public class UnknownObjectTypeException : ObjectExcpetion
    {
        public override string Message => UnknownTypeError;

        public UnknownObjectTypeException() : base() { }
    }
}
