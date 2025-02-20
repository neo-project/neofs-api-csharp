// Copyright (C) 2015-2025 The Neo Project.
//
// ObjectException.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

#nullable enable
using System;

namespace Neo.FileStorage.API.Object
{
    public class ObjectException : Exception
    {
        public const string AlreadyRemovedError = "object already removed";
        public const string RangeOutOfBoundsError = "payload range is out of bounds";
        public const string NotFoundError = "object not found";
        public const string SizeExceedLimitError = "payload size is greater than the limit";
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
