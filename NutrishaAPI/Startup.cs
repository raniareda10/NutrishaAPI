using System;
using System.IO;
using DL;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using NutrishaAPI.Jobs;
using NutrishaAPI.Middlewares;
using NutrishaAPI.ServicesRegistrations;
using Quartz;

// using Hangfire;

namespace NutrishaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterLegacyService(Configuration);
            services.RegisterDataAccessServices();
            services.RegisterCurrentUserServices();
            services.AddHttpClient();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(Configuration);
            });

            services.AddQuartz(scheduler =>
            {
                scheduler.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey("CheckSubscriptionsJob");
                scheduler.AddJob<CheckSubscriptionsJob>(opt => { opt.WithIdentity(jobKey); });
                scheduler.AddTrigger(options =>
                {
                    options.ForJob(jobKey)
                        .WithSimpleSchedule(builder =>
                        {
                            builder.RepeatForever()
                                .WithIntervalInMinutes(
                                    int.Parse(Configuration["CheckSubscriptionsJob:TriggerIntervalInMinutes"]));
                        });
                });
            });
            services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "./fire-store-credentials.json");
            var projectId = Configuration["ForeStore:ProjectId"];
            services.AddTransient<FirestoreDb>(options => FirestoreDb.Create(projectId));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            // IBackgroundJobClient backgroundJobClient,
            // IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Context.Response.Headers.Add("Content-Disposition", "attachment");
                } 
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //app.UseHangfireDashboard();
            //backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hanfire job!"));
            //recurringJobManager.AddOrUpdate(
            //    "Run every minute",
            //    () => serviceProvider.GetService<IClearFireBaseJob>().ClearOfferAsync(),
            //    "* * * * *"
            //    );
        }
    }
}