using System;
using System.Diagnostics;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    [DebuggerDisplay("UserId = {UserId}; Password = {Password}")]
    public class RequestAuthorization
    {
        [ProtoMember(1)]
        public string UserId { get; set; }
        [ProtoMember(2)]
        public string Password { get; set; }
    }
}
