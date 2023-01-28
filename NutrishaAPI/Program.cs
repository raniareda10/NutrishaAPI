using System;
using System.Threading.Tasks;
using DL.DBContext;
using DL.Entities;
using DL.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;

namespace NutrishaAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("Start Program.");
                var host = CreateHostBuilder(args).Build();

                var env = host.Services.GetRequiredService<IWebHostEnvironment>();
                if (!env.IsProduction())
                {
                    await host.RunAsync();
                    return;
                }

                const string ownerEmail = "team@nutrisha.app";
                const string ownerPassword = "P?@ssword16191214";
                var context = host.Services.GetRequiredService<AppDBContext>();
                var ownerUserRegistered = await context.MUser.AnyAsync(u => u.Email == ownerEmail);
                if (!ownerUserRegistered)
                {
                    var owner = new MUser()
                    {
                        Email = ownerEmail,
                        Password = PasswordHasher.HashPassword(ownerPassword),
                        IsAdmin = true,
                    };
                    await context.AddAsync(owner);
                    await context.SaveChangesAsync();
                }

                await host.RunAsync();
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

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}