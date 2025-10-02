// Copyright (C) 2015-2025 The Neo Project.
//
// Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Session;
using System;

namespace Neo.FileStorage.API.Status
{
    public static class Helper
    {
        private static object ExceptionToError(Exception e)
        {
            return CommonFail.Internal;
        }

        public static void SetStatus(this IResponse resp, Exception e)
        {
            if (resp is null) throw new ArgumentNullException(nameof(resp));
            if (resp.MetaHeader is null)
            {
                resp.MetaHeader = new ResponseMetaHeader();
            }
            resp.MetaHeader.Status = new()
            {
                Code = Status.Globalize(ExceptionToError(e)),
                Message = e.Message
            };
        }
    }
}
