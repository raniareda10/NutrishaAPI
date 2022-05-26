using System;
using System.Text;
using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DBContext;
using DL.MailModels;
using DL.Mapping;
using Hangfire;
using Hangfire.MemoryStorage;
using HELPER;
using KSEEngineeringJobs;
using MailReader;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.ApiModels;
using NutrishaAPIAPI.Extensions;

namespace NutrishaAPI.ServicesRegistrations
{
    public static class LegacyServices
    {
        /// <summary>
        /// Legacy Code Written By Ahmed Hassan
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterLegacyService(this IServiceCollection services, IConfiguration configuration)
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
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(configuration["SqlServer:ConnectionString"]));
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

            services.Configure<TokenManagement>(configuration.GetSection("tokenManagement"));
            var token = configuration.GetSection("tokenManagement").Get<TokenManagement>();

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

             


            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            

            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WebAPI", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                config.OperationFilter<Startup.AuthResponsesOperationFilter>();
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
    }
}