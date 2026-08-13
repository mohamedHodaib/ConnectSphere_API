using CleanArch.Infrastructure.Seed;
using ConnectSphere.Application.Common.Interfaces;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Settings;
using ConnectSphere.Application.Interfaces;
using ConnectSphere.Infrastructure.Identity;
using ConnectSphere.Infrastructure.Persistence;
using ConnectSphere.Infrastructure.Repositories;
using ConnectSphere.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // Register AppDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            //Rgister services
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<ITagService, TagService>();

            services.AddScoped<IRoleService, RoleService>();

            //for handling save files
            services.AddScoped<ISaveFile, SaveFile>();

            //Register Settings
            services.AddOptions<SmtpSettings>()
                .Bind(configuration.GetSection(SmtpSettings.Section));

            services.AddOptions<FrontendSettings>()
                .Bind(configuration.GetSection(FrontendSettings.Section));

            //Register Admin Info for using it at IdentitySeeder
            services.AddOptions<AdminInfo>()
                .Bind(configuration.GetSection(AdminInfo.Section));

            //Register seeding classes
            services.AddScoped<DatabaseInitializer>();
            services.AddScoped<IIdentitySeeder, IdentitySeeder>();
            services.AddHostedService<DatabaseInitializerHostedService>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //services for server url creation
            services.AddHttpContextAccessor();
            services.AddScoped<IUrlService, UrlService>();


            return services;
        }
    }
}
