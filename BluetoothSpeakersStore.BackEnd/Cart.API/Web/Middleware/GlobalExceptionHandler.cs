using Cart.API.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Cart.API.Web.Middleware
{
    internal class GlobalExceptionHandler
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
                    case CartNotFoundException cartNotFound:
                    case ProductNotFoundException productNotFound:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case CartCheckoutSumException cartSumException:
                    case UserDuplicateCartsException duplicateCartsException:
                    case UnsufficientProductQuantitiesException unsufficientProductQuantitiesException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UserUnsufficientBalance unsufficientBalanceExceptoion:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
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
