using Identity.API.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Identity.API.Web.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UserNotFoundException entity:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case DuplicateCredentialsException duplicateCredentialsException:
                    case UserRolesException userRolesException:
                    case UserSessionException userSessionException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case ProhibitedAdminAccountActionException exception:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case InfrastructureFailureException infrastructureFailureException:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(error?.Message);
                await response.WriteAsync(result);
            }
        }
    }
}
