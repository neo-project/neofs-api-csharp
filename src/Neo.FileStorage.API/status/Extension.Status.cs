using System;
using System.Collections.Generic;
using Grpc.Core;

namespace Neo.FileStorage.API.Status
{
    public sealed partial class Status
    {
        private static readonly Dictionary<Type, uint> sections = new()
        {
            { typeof(Success), 0 },
            { typeof(CommonFail), 1 }
        };

        private static readonly Dictionary<uint, Type> types = new()
        {
            { 0, typeof(Success) },
            { 1, typeof(CommonFail) }
        };

        public static uint Globalize(object ev)
        {
            if (sections.TryGetValue(ev.GetType(), out var section))
            {
                return (section << 10) + (uint)(int)ev;
            }
            throw new InvalidOperationException("invalid type");
        }

        public static object Localize(uint code)
        {
            var section = code / 1024;
            var ev = code % 1024;
            if (types.TryGetValue(section, out var t))
            {
                return Enum.ToObject(t, (int)ev);
            }
            throw new InvalidOperationException("invalid code");
        }

        public static bool InSection(uint code, Section i)
        {
            return code >= (uint)i << 10 && code < (uint)(i + 1) << 10;
        }

        public bool IsSuccess()
        {
            return InSection(Code, Section.Success);
        }

        public static StatusCode ToGrpcStatusCode(uint code)
        {
            var ev = Localize(code);
            return ev switch
            {
                Success.Ok => StatusCode.OK,
                CommonFail.Internal => StatusCode.Internal,
                _ => StatusCode.Unknown
            };
        }

        public Grpc.Core.Status ToGrpcStatus()
        {
            return new Grpc.Core.Status(ToGrpcStatusCode(Code), Message);
        }
    }
}
