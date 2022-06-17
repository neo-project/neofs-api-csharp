using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using Version = Neo.FileStorage.API.Refs.Version;

namespace Neo.FileStorage.API.Object;

public partial class Object
{
    public const int ChunkSize = 3 * (1 << 20);

    private Object parent;

    public Version Version => Header?.Version;
    public ulong PayloadSize => Header?.PayloadLength ?? 0;
    public ContainerID ContainerId => Header?.ContainerId;
    public OwnerID OwnerId => Header?.OwnerId;
    public ulong CreationEpoch => Header?.CreationEpoch ?? 0;

    public Checksum PayloadChecksum
    {
        get => Header?.PayloadHash;
        set => Header.PayloadHash = value;
    }

    public Checksum PayloadHomomorphicHash
    {
        get => Header?.HomomorphicHash;
        set => Header.HomomorphicHash = value;
    }

    public List<Header.Types.Attribute> Attributes => Header?.Attributes.ToList();
    public ObjectID PreviousId => Header?.Split?.Previous;

    public IEnumerable<ObjectID> Children
    {
        get => Header?.Split?.Children;
        set
        {
            if (Header is null) Header = new Header();
            if (Header.Split is null) Header.Split = new Header.Types.Split();
            Header.Split.Children.Clear();
            Header.Split.Children.AddRange(value);
        }
    }

    public SplitID SplitId
    {
        get => Header?.Split?.SplitId;
        set
        {
            if (Header is null) Header = new Header();
            if (Header.Split is null) Header.Split = new Header.Types.Split();
            Header.Split.SplitId = value.ToByteString();
        }
    }

    public ObjectID ParentId => Header?.Split?.Parent;
    public SessionToken SessionToken => Header?.SessionToken;

    public ObjectType ObjectType
    {
        get => Header.ObjectType;
        set => Header.ObjectType = value;
    }

    public bool HasParent => Header?.Split != null;
    public Address Address => new(ContainerId, ObjectId);

    public Object Parent
    {
        get
        {
            if (parent is not null) return parent;
            var split = Header?.Split;
            if (split is null) return null;
            if (split.ParentSignature is null && split.ParentHeader is null)
                return null;
            Object obj = new()
            {
                Header = split.ParentHeader,
                Signature = split.ParentSignature,
                ObjectId = split.Parent
            };
            return parent = obj;
        }
        set
        {
            parent = value;
            if (Header is null) Header = new Header();
            if (Header.Split is null) Header.Split = new Header.Types.Split();
            Header.Split.Parent = parent.ObjectId;
            Header.Split.ParentSignature = parent.Signature;
            Header.Split.ParentHeader = parent.Header;
        }
    }

    public ObjectID CalculateID()
    {
        return new ObjectID
        {
            Value = Header.Sha256()
        };
    }

    public bool VerifyID()
    {
        return CalculateID().Equals(ObjectId);
    }

    public Checksum CalculatePayloadChecksum(ChecksumType type)
    {
        if (Payload is null)
            throw new InvalidOperationException("cant calculate payload checksum: invalid payload");
        return type switch
        {
            ChecksumType.Sha256 => Payload.Sha256Checksum(),
            ChecksumType.Tz => Payload.TzChecksum(),
            _ => throw new InvalidOperationException("unsupport checksum type")
        };
    }

    public bool VerifyPayloadChecksum()
    {
        return CalculatePayloadChecksum(ChecksumType.Sha256).Equals(Header?.PayloadHash);
    }

    public Signature CalculateIDSignature(ECDsa key)
    {
        return key.SignMessagePart(ObjectId);
    }

    public bool VerifyIDSignature()
    {
        return Signature.VerifyMessagePart(ObjectId);
    }

    public void SetVerificationFields(ECDsa key)
    {
        Header.PayloadHash = CalculatePayloadChecksum(ChecksumType.Sha256);
        Header.HomomorphicHash = CalculatePayloadChecksum(ChecksumType.Tz);
        ObjectId = CalculateID();
        Signature = CalculateIDSignature(key);
    }

    public bool CheckVerificationFields()
    {
        if (!VerifyIDSignature()) return false;
        if (!VerifyID()) return false;
        return true;
    }

    public Object CutPayload()
    {
        var obj = Clone();
        obj.payload_ = ByteString.Empty;
        return obj;
    }

    public StorageGroup.StorageGroup GetStorageGroup()
    {
        if (ObjectType != ObjectType.StorageGroup)
            return null;
        return StorageGroup.StorageGroup.Parser.ParseFrom(Payload);
    }

    public Lock.Lock GetLock()
    {
        if (ObjectType != ObjectType.Lock)
            return null;
        return Lock.Lock.Parser.ParseFrom(Payload);
    }

    public Tombstone.Tombstone GetTombstone()
    {
        if (ObjectType != ObjectType.Tombstone)
            return null;
        return Tombstone.Tombstone.Parser.ParseFrom(Payload);
    }
}
