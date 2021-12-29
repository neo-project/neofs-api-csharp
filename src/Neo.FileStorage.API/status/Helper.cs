using System;
using Neo.FileStorage.API.Session;

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
