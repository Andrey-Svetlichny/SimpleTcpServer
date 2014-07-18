using System;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    public class RequestBuyCookie
    {
        [ProtoMember(1)]
        public Cookie Cookie { get; set; }
        [ProtoMember(2)]
        public uint Quantity { get; set; }
    }
}
