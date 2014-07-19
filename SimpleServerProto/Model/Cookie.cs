using System;
using System.Diagnostics;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    [DebuggerDisplay("Name = {Name}; Price = {Price}")]
    public class Cookie
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public uint Price { get; set; }
    }
}
