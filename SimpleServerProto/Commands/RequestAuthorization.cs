using System;
using System.Diagnostics;

namespace SimpleServerProto
{
    [Serializable]
    [DebuggerDisplay("UserId = {UserId}; Password = {Password}")]
    public class RequestAuthorization
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
