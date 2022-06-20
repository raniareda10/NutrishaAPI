using KSEEngineeringJobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;
using NLog;

namespace NutrishaAPIAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                logger.Info("Start Program.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
            }
            finally
            {
                LogManager.Flush();
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}