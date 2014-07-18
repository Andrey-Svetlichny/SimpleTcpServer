using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using SimpleServer.BinLayer;

namespace SimpleServer.TcpLayer
{
    public class TcpServer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly TcpListener _listener;

        public BinServer Server { get; set; }


        public TcpServer(string host, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(host), port);
        }

        public void Start()
        {
            _listener.Start();
            new Task(() =>
            {
                while (true)
                {
                    Log.Info("Waiting for a connection... ");
                    var client = _listener.AcceptTcpClient();
                    Log.Info("Connected!");
                    NetworkStream stream = client.GetStream();

                    // Buffer for reading data
                    var buffer = new Byte[256];
                    int count;
                    // Loop to receive all the data sent by the client.
                    while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        // Receive request
                        Log.DebugFormat("Received {0} bytes", count);
                        var requestBytes = new byte[count];
                        Array.Copy(buffer, requestBytes, count);

                        // Business logic
                        byte[] responseBytes = Server.Request(requestBytes);

                        // Send back a response.
                        stream.Write(responseBytes, 0, responseBytes.Length);
                        Log.DebugFormat("Sent {0} bytes", responseBytes.Length);
                    }

                    client.Close();
                    Log.Info("Close connection");
                }
            }).Start();
        }
    }
}