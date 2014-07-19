using System;
using System.Collections.Generic;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    public class ResponseAuthorization
    {
        [ProtoMember(1)]
        public bool Error { get; set; }
        [ProtoMember(2)]
        public string ErrorMessage { get; set; }
        [ProtoMember(3, IsRequired = true)]
        public uint Money { get; set; }
        [ProtoMember(4)]
        public Dictionary<Cookie, uint> Cookies { get; set; }
    }
}
