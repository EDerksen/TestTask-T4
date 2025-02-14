using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TestTask_T4.Exceptions;

namespace TestTask_T4.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhalded exception occured while processing request");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails problemDetails;
            if (exception is FinancialException financialException)
            {
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = financialException.Title,
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-422-unprocessable-content",
                    Extensions = financialException.Extensions!
                };
            }
            else if (exception is NotFoundException notFoundException)
            {
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = notFoundException.Title,
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found"
                };
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "An unexpected error occurred!",
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error"
                };
            }
            

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;

            var json = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(json);
        }
    }
}
