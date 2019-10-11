using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_with_nLog
{
    public class Bootstrapper
    {
        public static IServiceProvider Container { get; private set; }

        public Bootstrapper()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddLogger();
            services.AddSingleton(typeof(MainViewModel));

            Container = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return Container.GetService<MainViewModel>();
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddLogger(this ServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = folder + @"\Logs\${date:format=yyyy-MM-dd}.Player.log";

            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget("App.log")
            {
                FileName = fullPath,
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception}"
            };
            config.AddTarget(fileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, "App.log");

            LogManager.Configuration = config;
        }
    }
}
