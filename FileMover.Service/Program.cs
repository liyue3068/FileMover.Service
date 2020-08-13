using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileMover.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace FileMover.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var hostLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            //当应用退出后，Flush日志
            hostLifetime.ApplicationStopped.Register(() =>
            {
                NLog.LogManager.Shutdown();
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddHostedService<UDPService>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .UseNLog();
    }
}
