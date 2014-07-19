using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
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

            // читаем настройки
            var serverSettings = ConfigurationManager.GetSection("ServerSettings") as NameValueCollection;
            if (serverSettings == null)
            {
                throw new Exception("ServerSettings not found");
            }
            var host = serverSettings["host"];
            var port = int.Parse(serverSettings["port"]);

            // создаем сервер
            var tcpServer = new TcpServer(host, port);
            tcpServer.Start();

            // создаем несколько клиентов и запускаем их одновременно
            var tasks = new List<Task>();
            for (int i = 1; i < 10; i++)
            {
                var userId = "User" + i;
                var task = new Task(() =>
                {
                    var tcpClient = new TcpClient(host, port);
                    var binClient = new BinClient { Server = tcpClient };
                    tcpClient.Start();
                    var client = new SmartClient(userId, "123", 100) { Server = binClient };
                    client.Execute();
                    tcpClient.Stop();
                });
                task.Start();
                tasks.Add(task);
            }

            // ждем завершения работы всех клиентов
            Task.WaitAll(tasks.ToArray());

            tcpServer.Stop();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
