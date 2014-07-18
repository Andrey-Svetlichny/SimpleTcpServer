using System.IO;
using ProtoBuf;
using SimpleServerProto;

namespace SimpleServer.BinLayer
{
    /// <summary>
    /// Сериализация/десериализация обращений к Server,
    /// использует Protocol Buffers
    /// </summary>
    public class BinClient : IServer
    {
        public IBinServer Server { get; set; }
        public ResponseAuthorization Authorization(RequestAuthorization request)
        {
            // serialize
            var ms = new MemoryStream();
            Serializer.NonGeneric.SerializeWithLengthPrefix(ms, request, PrefixStyle.Base128, CommandMapper.Map(request));
            byte[] requestBytes = ms.ToArray();
            // call server
            byte[] responseBytes = Server.Request(requestBytes);
            // deserialize
            return Serializer.Deserialize<ResponseAuthorization>(new MemoryStream(responseBytes));
        }

        public ResponseCookieList CookieList()
        {
            var request = new RequestCookieList();
            // serialize
            var ms = new MemoryStream();
            Serializer.NonGeneric.SerializeWithLengthPrefix(ms, request, PrefixStyle.Base128, CommandMapper.Map(request));
            byte[] requestBytes = ms.ToArray();
            // call server
            byte[] responseBytes = Server.Request(requestBytes);
            // deserialize
            return Serializer.Deserialize<ResponseCookieList>(new MemoryStream(responseBytes));
        }

        public ResponseBuyCookie BuyCookie(RequestBuyCookie request)
        {
            // serialize
            var ms = new MemoryStream();
            Serializer.NonGeneric.SerializeWithLengthPrefix(ms, request, PrefixStyle.Base128, CommandMapper.Map(request));
            byte[] requestBytes = ms.ToArray();
            // call server
            byte[] responseBytes = Server.Request(requestBytes);
            // deserialize
            return Serializer.Deserialize<ResponseBuyCookie>(new MemoryStream(responseBytes));
        }
    }
}