using System.Security.Claims;

namespace Cart.API.Application.Commands.RemoveProductFromCart
{
    public interface IRemoveProductInCart
    {
        Task RemoveProductFromLoggedUserCartAsync(Guid productId, ClaimsPrincipal loggedUser);
    }
}