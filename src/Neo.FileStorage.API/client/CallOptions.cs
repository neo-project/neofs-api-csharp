using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Session;
using Neo.FileStorage.API.Refs;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;

namespace Neo.FileStorage.API.Client
{
    public class CallOptions
    {
        public Refs.Version Version;
        public uint Ttl;
        public ulong Epoch;
        public List<XHeader> XHeaders;
        public SessionToken Session;
        public BearerToken Bearer;
        public ECDsa Key;

        public RequestMetaHeader GetRequestMetaHeader()
        {
            var meta = new RequestMetaHeader
            {
                Version = Version,
                Ttl = Ttl,
                Epoch = Epoch,
            };
            if (XHeaders is not null) meta.XHeaders.AddRange(XHeaders);
            if (Session is not null) meta.SessionToken = Session;
            if (Bearer is not null) meta.BearerToken = Bearer;
            return meta;
        }

        public CallOptions ApplyCustomOptions(CallOptions custom)
        {
            if (custom is null) return this;
            if (custom.Version is not null) Version = custom.Version;
            Ttl = custom.Ttl;
            Epoch = custom.Epoch;
            if (custom.XHeaders is not null) XHeaders = custom.XHeaders;
            if (custom.Session is not null) Session = custom.Session;
            if (custom.Bearer is not null) Bearer = custom.Bearer;
            if (custom.Key is not null) Key = custom.Key;
            return this;
        }

        public CallOptions WithVersion(Refs.Version v)
        {
            Version = v;
            return this;
        }

        public CallOptions WithTTL(uint ttl)
        {
            Ttl = ttl;
            return this;
        }

        public CallOptions WithEpoch(ulong epoch)
        {
            Epoch = epoch;
            return this;
        }

        public CallOptions WithXHeaders(IEnumerable<XHeader> xheaders)
        {
            if (XHeaders is null)
                XHeaders = new();
            else
                XHeaders.Clear();
            XHeaders.AddRange(xheaders);
            return this;
        }

        public CallOptions WithExtraXHeaders(IEnumerable<XHeader> xheaders)
        {
            if (XHeaders is null)
                XHeaders = new();
            XHeaders.AddRange(xheaders);
            return this;
        }

        public CallOptions WithSessionToken(SessionToken session)
        {
            Session = session;
            return this;
        }

        public CallOptions WithBearerToken(BearerToken bearer)
        {
            Bearer = bearer;
            return this;
        }

        public CallOptions WithKey(ECDsa key)
        {
            Key = key;
            return this;
        }
    }
}
