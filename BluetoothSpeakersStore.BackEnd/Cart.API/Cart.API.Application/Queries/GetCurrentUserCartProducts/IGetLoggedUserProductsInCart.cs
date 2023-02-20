using Cart.API.DTO.Response;
using System.Security.Claims;

namespace Cart.API.Application.Queries.GetCurrentUserCartProducts
{
    public interface IGetLoggedUserProductsInCart
    {
        Task<IEnumerable<GetCartProductsModel>> GetLoggedUserCartProductsAsync(ClaimsPrincipal loggedUser);
    }
}