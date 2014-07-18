using System;
using System.Collections.Specialized;
using System.Configuration;
using SimpleServer;
using SimpleServer.BinLayer;
using SimpleServer.TcpLayer;

namespace TestTcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TestTcpServer");

            var serverSettings = ConfigurationManager.GetSection("ServerSettings") as NameValueCollection;
            if (serverSettings == null)
            {
                throw new Exception("ServerSettings not found");
            }
            var host = serverSettings["host"];
            var port = int.Parse(serverSettings["port"]);

            var server = new Server();
            var binServer = new BinServer { Server = server };
            var tcpServer = new TcpServer(host, port) { Server = binServer };
            var tcpClient = new TcpClient(host, port);
            var binClient = new BinClient { Server = tcpClient };
            var client = new Client { Server = binClient };
            tcpServer.Start();
            tcpClient.Start();

            client.Start();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
