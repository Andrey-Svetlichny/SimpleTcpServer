using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SimpleServer.BinLayer;

namespace SimpleServer.TcpLayer
{
    public class TcpServer : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly CancellationTokenSource _cancellationTokenSource;
        readonly TcpListener _listener;

        public TcpServer(string host, int port)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Parse(host), port);
        }

        public void Start()
        {
            var token = _cancellationTokenSource.Token;
            _listener.Start();
            // создание потока сервера
            new Task(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Log.Info("Waiting for a connection... ");
                    var client = _listener.AcceptTcpClient();
                    Log.Info("Connected!");

                    // создание потока для обработки запросов одного клиента
                    new Task(() =>
                    {
                        var server = new Server();
                        var binServer = new BinServer { Server = server };
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
                            byte[] responseBytes = binServer.Request(requestBytes);

                            // Send back a response.
                            stream.Write(responseBytes, 0, responseBytes.Length);
                            Log.DebugFormat("Sent {0} bytes", responseBytes.Length);
                        }

                        client.Close();
                        Log.Info("Close connection");
                    }).Start();
                }
            }).Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
        }
    }
}