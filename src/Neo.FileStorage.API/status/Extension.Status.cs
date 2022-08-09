using System;
using System.Collections.Generic;
using Grpc.Core;

namespace Neo.FileStorage.API.Status;

public sealed partial class Status
{
    private static readonly Dictionary<Type, Section> Sections = new()
    {
        { typeof(Success), Section.Success },
        { typeof(CommonFail), Section.FailureCommon },
        { typeof(Object), Section.Object },
        { typeof(Container), Section.Container },
        { typeof(Session), Section.Session }
    };

    private static readonly Dictionary<Section, Type> SectionTypes = new()
    {
        { Section.Success, typeof(Success) },
        { Section.FailureCommon, typeof(CommonFail) },
        { Section.Object, typeof(Object) },
        { Section.Container, typeof(Container) },
        { Section.Session, typeof(Session) }
    };

    public static uint Globalize(object ev)
    {
        if (Sections.TryGetValue(ev.GetType(), out var section)) return ((uint)(int)section << 10) + (uint)(int)ev;

        throw new InvalidOperationException("invalid type");
    }

    public static object Localize(uint code)
    {
        var section = (Section)(code / 1024);
        var ev = code % 1024;
        if (SectionTypes.TryGetValue(section, out var t)) return Enum.ToObject(t, ev);

        throw new InvalidOperationException("invalid code");
    }

    private static bool InSection(uint code, Section i)
    {
        return code >= (int)i << 10 && code < (int)(i + 1) << 10;
    }

    public bool IsSuccess()
    {
        return InSection(Code, Section.Success);
    }

    private static StatusCode ToGrpcStatusCode(uint code)
    {
        var ev = Localize(code);
        return ev switch
        {
            Success.Ok => StatusCode.OK,
            CommonFail.Internal => StatusCode.Internal,
            Object.AccessDenied => StatusCode.PermissionDenied,
            Object.NotFound => StatusCode.NotFound,
            Container.NotFound => StatusCode.NotFound,
            Session.TokenExpired => StatusCode.Unauthenticated,
            _ => StatusCode.Unknown
        };
    }

    public Grpc.Core.Status ToGrpcStatus()
    {
        return new Grpc.Core.Status(ToGrpcStatusCode(Code), Message);
    }
}
