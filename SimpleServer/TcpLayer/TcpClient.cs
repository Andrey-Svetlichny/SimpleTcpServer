using System;
using System.Net.Sockets;
using System.Reflection;
using log4net;
using SimpleServer.BinLayer;

namespace SimpleServer.TcpLayer
{
    public class TcpClient : IBinServer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        System.Net.Sockets.TcpClient _client;
        NetworkStream _stream;
        public void Start()
        {
            const string host = "127.0.0.1";
            const int port = 82;
            _client = new System.Net.Sockets.TcpClient(host, port);
            _stream = _client.GetStream();
        }

        public void Stop()
        {
            _stream.Close();
            _client.Close();
        }

        public byte[] Request(byte[] request)
        {
            // Send request
            _stream.Write(request, 0, request.Length);
            Log.DebugFormat("Sent {0} bytes", request.Length);

            // Receive response
            var data = new Byte[256];
            var count = _stream.Read(data, 0, data.Length);
            Array.Resize(ref data, count);
            Log.DebugFormat("Received {0} bytes", data.Length);
            return data;
        }
    }
}