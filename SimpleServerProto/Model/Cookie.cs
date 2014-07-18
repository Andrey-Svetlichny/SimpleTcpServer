using System;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    public class Cookie
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public uint Price { get; set; }
    }
}
