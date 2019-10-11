using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UWP_with_nLog
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

            // UWP is very restrictive of where you can save files on the disk.
            // The preferred place to do that is app's local folder.
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string fullPath = folder.Path + @"\Logs\${date:format=yyyy-MM-dd}.Player.log";

            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget("App.log")
            {
                FileName = fullPath,
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception}",
                ConcurrentWrites = false
            };
            config.AddTarget(fileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, "App.log");

            LogManager.Configuration = config;

            // Add internal logging.
            InternalLogger.LogFile = folder.Path + @"\Logs\internal.nLog.log";
            InternalLogger.LogLevel = NLog.LogLevel.Trace;
        }
    }
}
