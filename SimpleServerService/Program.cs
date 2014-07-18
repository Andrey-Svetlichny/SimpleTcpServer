using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using log4net;
using SimpleServer.TcpLayer;

namespace SimpleServerService
{
    /// <summary>
    /// Консольное приложение для тестирования и Windows Service для реального использования.
    /// Хост для SimpleServer.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    class Program : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static TcpServer _tcpServer;

        public Program()
        {
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        static void Main(string[] args)
        {
            if (IsService())
            {
                Run(new Program());
            }
            else
                RunAsConsoleApp();
        }


        protected static void RunAsConsoleApp()
        {
#if DEBUG
            // запускаем в основном потоке
            Log.Info("Debugging.");
            try
            {
                StartService();
                Console.WriteLine("The service is ready.\nPress <Esc> to terminate service.\n");
                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                StopService();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                Abort();
            }
#else
            // запускаем по настоящему
            var service = new Program();

            service.OnStart(null);
            Console.WriteLine("Press <Esc> to exit...");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
            Console.WriteLine("Stopping...");
            service.OnStop();
#endif
        }


        private static void Abort()
        {
            Log.Debug("Abort");
            _tcpServer.Stop();
        }

        static void StopService()
        {
            Log.Debug("StopService");
            _tcpServer.Stop();
        }

        static void StartService()
        {
            Log.Debug("StartService");

            // проверяем настройки log4net и пишем номер версии
            Log.InfoFormat("Version = {0}", Assembly.GetExecutingAssembly().GetName().Version);

            try
            {
                var serverSettings = ConfigurationManager.GetSection("ServerSettings") as NameValueCollection;
                if (serverSettings == null)
                {
                    throw new Exception("ServerSettings not found");
                }
                var host = serverSettings["host"];
                var port = int.Parse(serverSettings["port"]);

                _tcpServer = new TcpServer(host, port);
                _tcpServer.Start();

                Log.Info("Service started.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }
        }


        protected override void OnStart(string[] args)
        {
            Log.Info("Starting.");
            try
            {
                StartService();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
                OnStop();
            }
        }


        protected override void OnStop()
        {
            Log.Info("Stopping.");
            StopService();
        }


        protected static bool IsService()
        {
            return String.Compare(Environment.CurrentDirectory, Environment.SystemDirectory, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
