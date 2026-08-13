using ConnectSphere.Application;
using ConnectSphere.WebApi.Middleware;
using Serilog;
//For Applying [ApiController] to all controllers in the assembly
using Microsoft.AspNetCore.Mvc;
using ConnectSphere.Infrastructure;
using ConnectSphere.API.Extentions;
using ConnectSphere.API.ExceptionHandlers;
using System.Threading.Tasks;
using Scalar.AspNetCore;
using ConnectSphere.Infrastructure.Identity;
using System.Text.Json.Serialization;

[assembly: ApiController]

namespace ConnectSphere.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Serilog .
            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.WriteTo.Console();
                loggerConfiguration.WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                );

                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            });

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //Register problem details service
            builder.Services.AddProblemDetails();

            //Register  Exception Handlers
            builder.Services.AddExceptionHandler<FluentValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Add Application Layer
            builder.Services.AddApplication(builder.Configuration);

            builder.Services.AddIdentityAndAuthentication(builder.Configuration);

            // Add Infrastructure Layer
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin(); // for development only
                });
            });

            

            var app = builder.Build();

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }


    }
}
