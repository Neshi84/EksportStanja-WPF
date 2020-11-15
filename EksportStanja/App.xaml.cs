using EksportStanja.Repository;
using EksportStanja.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace EksportStanja
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.Configure<DatabaseOptions>(Configuration.GetSection("ConnectionStrings"));
            serviceCollection.AddTransient<IUtrosakRepository, UtrosakRepository>();
            serviceCollection.AddTransient<ISifrarnikRepository, SifrarnikRepository>();
            serviceCollection.AddTransient<IExcelService, ExcelService>();
            serviceCollection.AddTransient<IXmlService, XmlService>();
        }
    }
}