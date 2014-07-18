using System;
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

            var server = new Server();
            var binServer = new BinServer { Server = server };
            var tcpServer = new TcpServer { Server = binServer };
            var tcpClient = new TcpClient();
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
