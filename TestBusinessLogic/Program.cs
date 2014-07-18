using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var server = new Server();
            var client = new Client { Server = server };
            client.Start();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
