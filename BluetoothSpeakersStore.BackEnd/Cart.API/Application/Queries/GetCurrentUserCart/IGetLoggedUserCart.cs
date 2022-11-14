using Cart.API.DTO.Response;
using System.Security.Claims;

namespace Cart.API.Application.Queries.GetCurrentUserCart
{
    public interface IGetLoggedUserCart
    {
        Task<GetUserCartModel> GetLoggedUserCartAsync(ClaimsPrincipal loggedUser);
    }
}