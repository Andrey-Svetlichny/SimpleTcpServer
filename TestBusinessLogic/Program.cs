using System;
using SimpleServer;

namespace TestBusinessLogic
{
    /// <summary>
    /// Тест логики работы сервера.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TestBusinessLogic");

            var server = new Server();
            var client = new Client { Server = server };
            client.Start();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
