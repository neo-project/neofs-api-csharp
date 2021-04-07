using Google.Protobuf;
using Neo.IO.Json;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Session;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace Neo.FileStorage.API.Object
{
    public partial class Object
    {
        public const int ChunkSize = 3 * (1 << 20);

        public Version Version => Header?.Version;
        public ulong PayloadSize => Header?.PayloadLength ?? 0;
        public ContainerID ContainerId => Header?.ContainerId;
        public OwnerID OwnerId => Header?.OwnerId;
        public ulong CreationEpoch => Header?.CreationEpoch ?? 0;
        public Checksum PayloadChecksum => Header?.PayloadHash;
        public Checksum PayloadHomomorphicHash => Header?.HomomorphicHash;
        public List<Header.Types.Attribute> Attributes => Header?.Attributes.ToList();
        public ObjectID PreviousId => Header?.Split?.Previous;
        public List<ObjectID> Children => Header?.Split?.Children?.ToList();
        public SplitID SplitId => Header?.Split?.SplitId is null ? null : new SplitID(Header.Split.SplitId);
        public ObjectID ParentId => Header?.Split?.Parent;
        public SessionToken SessionToken => Header?.SessionToken;
        public ObjectType ObjectType => Header.ObjectType;
        public bool HasParent => Header?.Split != null;
        public Address Address => new(ContainerId, ObjectId);

        private Object parent = null;
        public Object Parent
        {
            get
            {
                if (parent is not null) return parent;
                var splitHeader = Header?.Split;
                if (splitHeader is null) return null;
                var parentSig = splitHeader.ParentSignature;
                var parentHeader = splitHeader.ParentHeader;
                if (parentSig is null || parentHeader is null)
                    return null;
                Object obj = new()
                {
                    Header = parentHeader,
                    Signature = parentSig,
                };
                return parent = obj;
            }
        }

        public ObjectID CalculateAndGetID
        {
            get
            {
                if (objectId_ is null)
                {
                    objectId_ = new ObjectID
                    {
                        Value = Header.Sha256()
                    };
                }
                return objectId_;
            }
        }

        private ObjectID CalculateID()
        {
            return new ObjectID
            {
                Value = Header.Sha256()
            };
        }

        public bool VerifyID()
        {
            return CalculateID() == ObjectId;
        }

        public Checksum CalculatePayloadChecksum()
        {
            if (Payload is null || Payload.Length == 0)
                throw new System.InvalidOperationException("cant payload checksum: invalid payload");
            return Payload.Sha256Checksum();
        }

        public bool VerifyPayloadChecksum()
        {
            return CalculatePayloadChecksum().Equals(Header?.PayloadHash);
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
            Header.PayloadHash = CalculatePayloadChecksum();
            ObjectId = CalculateID();
            Signature = CalculateIDSignature(key);
        }

        public bool CheckVerificationFields()
        {
            if (!VerifyIDSignature()) return false;
            if (!VerifyID()) return false;
            if (!VerifyPayloadChecksum()) return false;
            return true;
        }

        public Object CutPayload()
        {
            var obj = Parser.ParseFrom(this.ToByteArray());
            obj.payload_ = ByteString.Empty;
            return obj;
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["objectid"] = ObjectId?.ToJson();
            json["signature"] = Signature?.ToJson();
            json["header"] = Header?.ToJson();
            json["payload"] = Payload?.ToBase64();
            return json;
        }
    }
}
