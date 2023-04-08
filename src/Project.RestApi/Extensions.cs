using System;
using System.IO;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Domain.Exceptions;
using Serilog;

namespace Project.RestApi
{
    public static class Extensions
    {
        public static void AddLogging(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            serviceCollection.AddLogging(i =>
            {
                i.ClearProviders();
                i.AddSerilog(logger);
            });
        }

        public static void UseGlobalException(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseGlobalExceptionHandler(
                i =>
                {
                    i.ContentType = "application/json";
                    i.ResponseBody(_ => JsonConvert.SerializeObject(new {errorMessage = "An error occurred."}));
                    i.Map<FileNotFoundException>().ToStatusCode(StatusCodes.Status400BadRequest).WithBody(
                        (ex, _) => JsonConvert.SerializeObject(new {errorMessage = ex.Message}));
                    i.Map<DomainException>().ToStatusCode(StatusCodes.Status400BadRequest).WithBody(
                        (ex, _) => JsonConvert.SerializeObject(new {errorMessage = ex.Message}));
                    i.Map<ArgumentException>().ToStatusCode(StatusCodes.Status400BadRequest).WithBody(
                        (ex, _) => JsonConvert.SerializeObject(new {errorMessage = ex.Message}));
                    i.Map<DbUpdateException>().ToStatusCode(StatusCodes.Status400BadRequest).WithBody(
                        (_, _) => JsonConvert.SerializeObject(new {errorMessage = "BadRequest"}));
                    i.Map<NotSupportedException>().ToStatusCode(StatusCodes.Status501NotImplemented).WithBody(
                        (_, _) => JsonConvert.SerializeObject(new
                            {errorMessage = "This feature is not currently supported."}));
                    i.Map<NotImplementedException>().ToStatusCode(StatusCodes.Status501NotImplemented).WithBody(
                        (_, _) => JsonConvert.SerializeObject(new
                            {errorMessage = "This feature will be implemented as soon as possible."}));
                });
        }
    }
}