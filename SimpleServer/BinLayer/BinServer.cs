using System.IO;
using System.Runtime.Serialization;
using ProtoBuf;
using SimpleServerProto;

namespace SimpleServer.BinLayer
{
    public interface IBinServer
    {
        byte[] Request(byte[] request);
    }

    /// <summary>
    /// Сериализация/десериализация обращений к Server,
    /// использует Protocol Buffers
    /// </summary>
    public class BinServer : IBinServer
    {
        public Server Server { get; set; }

        public byte[] Request(byte[] request)
        {
            var inStream = new MemoryStream(request);
            var outStream = new MemoryStream();
            object obj;
            if (!Serializer.NonGeneric.TryDeserializeWithLengthPrefix(inStream, PrefixStyle.Base128, CommandMapper.Map, out obj))
            {
                throw new SerializationException("Can't desirialize");
            }


            if (obj is RequestAuthorization)
            {
                var response = Server.Authorization((RequestAuthorization)obj);
                Serializer.Serialize(outStream, response);
            }
            else if (obj is RequestCookieList)
            {
                var response = Server.CookieList();
                Serializer.Serialize(outStream, response);
            }
            else if (obj is RequestBuyCookie)
            {
                var response = Server.BuyCookie((RequestBuyCookie)obj);
                Serializer.Serialize(outStream, response);
            }
            return outStream.ToArray();
        }
    }
}