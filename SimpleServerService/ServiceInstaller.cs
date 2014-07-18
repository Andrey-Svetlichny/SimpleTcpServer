using System.ComponentModel;
using System.ServiceProcess;

namespace SimpleServerService
{
    [RunInstaller(true)]
    [System.ComponentModel.DesignerCategory("")]
    public class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };

            var serviceInstaller = new System.ServiceProcess.ServiceInstaller
            {
                Description = "Тестовый TCP Server продажи печенек",
                DisplayName = "Svetlichny: SimpleTcpServer",
                ServiceName = "Svetlichny.SimpleTcpServer",
                StartType = ServiceStartMode.Automatic
            };

            Installers.AddRange(new System.Configuration.Install.Installer[] {
            serviceProcessInstaller,
            serviceInstaller});
        }
    }
}
