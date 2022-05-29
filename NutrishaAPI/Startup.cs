using BL.Infrastructure;
using BL.Security;
using DL.DBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DL.Mapping;
using DL.MailModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ElmahCore;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using ElmahCore.Mvc.Notifiers;
using NutrishaAPIAPI.Extensions;
using System.IO;
using DL;
using NLog;
using HELPER;
using MailReader;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hangfire;
using Hangfire.MemoryStorage;
using NutrishaAPI.Middlewares;
using NutrishaAPI.ServicesRegistrations;
using NutrishaAPIAPI;

namespace KSEEngineeringJobs
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
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            // app.UseElmah();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();
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

        // AuthResponsesOperationFilter.cs
        public class AuthResponsesOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>();

                if (authAttributes.Any())
                {
                    var securityRequirement = new OpenApiSecurityRequirement()
                    {
                        {
                            // Put here you own security scheme, this one is an example
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    };
                    operation.Security = new List<OpenApiSecurityRequirement> {securityRequirement};
                    operation.Responses.Add("401", new OpenApiResponse {Description = "Unauthorized"});
                }
            }
        }
    }
}