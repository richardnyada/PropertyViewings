using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PropertyViewings.Api;

public static class ApiExceptionMappingExtensions
{
    public static IApplicationBuilder UseApiExceptionMapping(this IApplicationBuilder app)
    {
        return app.Use(async (ctx, next) =>
        {
            try
            {
                await next();
            }
            catch (ValidationException ex)
            {
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                var problem = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Type = "https://httpstatuses.com/400",
                    Instance = ctx.Request.Path
                };

                await ctx.Response.WriteAsJsonAsync(problem);
            }
            catch (InvalidOperationException ex)
            {
                // e.g. slot already booked
                ctx.Response.StatusCode = StatusCodes.Status409Conflict;

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.com/409",
                    Instance = ctx.Request.Path
                };

                await ctx.Response.WriteAsJsonAsync(problem);
            }
            catch (Exception ex)
            {
                var logger = ctx.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("ApiExceptionMapping");
                logger.LogError(ex, "Unhandled exception");

                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred.",
                    Type = "https://httpstatuses.com/500",
                    Instance = ctx.Request.Path
                };

                await ctx.Response.WriteAsJsonAsync(problem);
            }
        });
    }
}
