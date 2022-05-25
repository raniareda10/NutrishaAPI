
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
using NLog;
using HELPER;
using MailReader;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hangfire;
using Hangfire.MemoryStorage;
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
            #region Configure Logging

           // services.AddElmah<SqlErrorLog>(options =>
           //{
           //    options.ConnectionString = Configuration.GetConnectionString("ConnectionString");

           //    options.ApplicationName = Configuration["Ksejos"];

           //});

            #endregion
            services.ConfigureLoggerService();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddHangfire(config =>
               config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseDefaultTypeSerializer()
               .UseMemoryStorage());

            services.AddHangfireServer();

         //   services.AddTransient<IClearFireBaseJob, ClearFireBaseJob>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISMS, SMS>();
            services.AddTransient<IMailRepository, MailRepository>();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingConfigration());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
          
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #region Configure Session
            services.AddDistributedMemoryCache();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(
                options =>
                {
                    options.Cookie.IsEssential = true;
                    options.Cookie.HttpOnly = true;
                    options.IdleTimeout = TimeSpan.FromHours(10);
                }
            );
            #endregion
            #region API Token Config

            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();

            services.AddAuthentication(jwtBearerDefaults =>
            {
                jwtBearerDefaults.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                jwtBearerDefaults.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    // ValidIssuer = token.Issuer,
                    // ValidAudience = token.Audience
                };
            });

            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();

            #endregion


            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<ICheckUniqes, ChekUniqeSer>();
            services.AddScoped<IUserManagementService, UserManagementService>();

             


            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddControllers();

            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WebAPI", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                config.OperationFilter<AuthResponsesOperationFilter>();
                //config.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment()||env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
                    operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                }
            }
        }
    }
}
