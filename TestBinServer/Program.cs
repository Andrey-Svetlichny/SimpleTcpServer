using System;
using SimpleServer;
using SimpleServer.BinLayer;

namespace TestBinServer
{
    /// <summary>
    /// Тест работы BinServer.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TestBinServer");

            var server = new Server();
            var binServer = new BinServer { Server = server };
            var binClient = new BinClient { Server = binServer };
            var client = new Client { Server = binClient };
            client.Start();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
