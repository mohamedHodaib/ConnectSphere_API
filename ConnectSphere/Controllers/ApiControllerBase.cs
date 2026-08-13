using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Text.Json;

namespace ConnectSphere.API.Controllers
{

    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult HandleCreatedResult<T>(Result<T> result, string actionName, object routeValues)
        {
            //Handle errors immediately to reduce nesting
            if (!result.IsSuccess)
            {
                return HandleFailure(result.Errors);
            }

            // HTTP 201 Created is returned.
            // It requires the URI of the newly created resource.
            return CreatedAtAction(actionName, routeValues, result.Data);
        }

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            //Handle errors immediately to reduce nesting
            if (!result.IsSuccess)
            {
                return HandleFailure(result.Errors);
            }
            
            if (typeof(T) == typeof(Unit))
            {
                return NoContent();
            }

            return Ok(result.Data);
        }

        protected IActionResult HandleResult(Result result)
        {
            //Handle errors immediately to reduce nesting
            if (!result.IsSuccess)
            {
                return HandleFailure(result.Errors);
            }

            return NoContent();
        }


        protected IActionResult HandlePagedResult<T>(Result<PagedResult<T>> pagedResult)
        {
            if(!pagedResult.IsSuccess)
            {
                return HandleFailure(pagedResult.Errors);
            }

            Response.Headers.TryAdd("X-Pagination",
            JsonSerializer.Serialize(pagedResult.Data!.MetaData));

            return Ok(pagedResult.Data.Items);
        }


        private IActionResult HandleFailure(IReadOnlyList<Error> errors)
        {
            // We can inspect the *first* error to decide on the status code
            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            
            return Problem(
                title: GetDefaultTitleForStatusCode(statusCode),
                statusCode: statusCode,
                type: GetDefaultTypeForStatusCode(statusCode),
                detail: $"{firstError.Code}: {firstError.Description}" 
            );
        }

        private static string GetDefaultTitleForStatusCode(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            406 => "Not Acceptable",
            409 => "Conflict",
            415 => "Unsupported Media Type",
            422 => "Unprocessable Entity",
            429 => "Too Many Requests",
            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            504 => "Gateway Timeout",
            _ => "An error occurred"
        };

        private static string GetDefaultTypeForStatusCode(int statusCode) => statusCode switch
        {
            400 => "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request",
            401 => "https://datatracker.ietf.org/doc/html/rfc9110#name-401-unauthorized",
            403 => "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden",
            404 => "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found",
            405 => "https://datatracker.ietf.org/doc/html/rfc9110#name-405-method-not-allowed",
            406 => "https://datatracker.ietf.org/doc/html/rfc9110#name-406-not-acceptable",
            409 => "https://datatracker.ietf.org/doc/html/rfc9110#name-409-conflict",
            415 => "https://datatracker.ietf.org/doc/html/rfc9110#name-415-unsupported-media-type",
            422 => "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
            429 => "https://datatracker.ietf.org/doc/html/rfc6585#section-4",
            500 => "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
            501 => "https://datatracker.ietf.org/doc/html/rfc9110#name-501-not-implemented",
            502 => "https://datatracker.ietf.org/doc/html/rfc9110#name-502-bad-gateway",
            503 => "https://datatracker.ietf.org/doc/html/rfc9110#name-503-service-unavailable",
            504 => "https://datatracker.ietf.org/doc/html/rfc9110#name-504-gateway-timeout",
            _ => "https://datatracker.ietf.org/doc/html/rfc9110"
        };
    }

}
