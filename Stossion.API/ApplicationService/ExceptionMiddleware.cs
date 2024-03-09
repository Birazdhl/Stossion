namespace Stossion.API.ExceptionHandler
{
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
    using Stossion.BusinessLayers.Interfaces;
    using Stossion.DbManagement.StossionDbManagement;
    using Stossion.Domain;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context, StossionDbContext dbContext, IUserInterface _userInterface)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                var user = _userInterface.GetUserDetails();


                // Log to the database
                dbContext.ErrorLogs.Add(new ErrorLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace ?? string.Empty,
                    DateTime = DateTime.Now,
                    Username = user?.UserName ?? string.Empty,
                });

                await dbContext.SaveChangesAsync();

                // Rethrow the exception to maintain the original behavior
                throw;
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

