using System;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    public class ResponseBuyCookie
    {
        [ProtoMember(1)]
        public bool Error { get; set; }
        [ProtoMember(2)]
        public string ErrorMessage { get; set; }
        [ProtoMember(3)]
        public Cookie Cookie { get; set; }
        [ProtoMember(4)]
        public uint Quantity { get; set; }
    }
}
