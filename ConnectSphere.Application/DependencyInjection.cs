using ConnectSphere.Application.Common.Behaviors;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Settings;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //Register For Using Handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
            });


            services.AddValidatorsFromAssembly(assembly);

            services.AddAutoMapper(cfg => { }, assembly);

            //Settings for using Options Pattern
            services.AddOptions<ValidationSettings>()
                .Bind(configuration.GetSection(ValidationSettings.Section));

            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.Section));

            return services;
        }
    }
}
