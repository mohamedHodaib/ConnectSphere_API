using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectSphere.API.ExceptionHandlers
{
    public class FluentValidationExceptionHandler(ILogger<FluentValidationExceptionHandler> logger
        , IProblemDetailsService problemDetailsService) : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            if (exception is not FluentValidation.ValidationException fluentException)
            {
                return false;
            }

            var problemDetails = new ProblemDetails();

            problemDetails.Title = "one or more validation errors occurred.";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            List<string> validationErrors = new List<string>();
            foreach (var error in fluentException.Errors)
            {
                validationErrors.Add(error.ErrorMessage);
            }
            problemDetails.Extensions.Add("errors", validationErrors);

            logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });

            return true;
        }
    }
}
