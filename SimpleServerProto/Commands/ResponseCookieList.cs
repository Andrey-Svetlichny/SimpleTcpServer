using System;
using System.Collections.Generic;
using ProtoBuf;

namespace SimpleServerProto
{
    [ProtoContract]
    [Serializable]
    public class ResponseCookieList
    {
        [ProtoMember(1)]
        public List<Cookie> Cookies { get; set; }
    }
}
