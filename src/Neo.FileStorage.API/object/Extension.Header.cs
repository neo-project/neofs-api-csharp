// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Header.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Object
{
    public sealed partial class Header
    {
        public static partial class Types
        {
            public sealed partial class Split
            {

            }

            public sealed partial class Attribute
            {
                // SysAttributePrefix is a prefix of key to system attribute.
                public const string SysAttributePrefix = "__NEOFS__";

                // SysAttributeUploadID marks smaller parts of a split bigger object.
                public const string SysAttributeUploadID = SysAttributePrefix + "UPLOAD_ID";

                // SysAttributeExpEpoch tells GC to delete object after that epoch.
                public const string SysAttributeExpEpoch = SysAttributePrefix + "EXPIRATION_EPOCH";

                // AttributeName is an attribute key that is commonly used to denote
                // human-friendly name.
                public const string AttributeName = "Name";

                // AttributeFileName is an attribute key that is commonly used to denote
                // file name to be associated with the object on saving.
                public const string AttributeFileName = "FileName";

                // AttributeTimestamp is an attribute key that is commonly used to denote
                // user-defined local time of object creation in Unix Timestamp format.
                public const string AttributeTimestamp = "Timestamp";

                // AttributeTimestamp is an attribute key that is commonly used to denote
                // MIME Content Type of object's payload.
                public const string AttributeContentType = "Content-Type";
            }
        }
    }
}
