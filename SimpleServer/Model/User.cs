using System.Collections.Generic;
using SimpleServerProto;

namespace SimpleServer.Model
{    
    class User
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public uint Money { get; set; }
        public Dictionary<Cookie, uint> Cookies { get; set; }
    }
}
