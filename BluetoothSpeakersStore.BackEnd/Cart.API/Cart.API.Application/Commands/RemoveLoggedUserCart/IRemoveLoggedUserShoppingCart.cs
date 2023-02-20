using System.Security.Claims;

namespace Cart.API.Application.Commands.RemoveLoggedUserCart
{
    public interface IRemoveLoggedUserShoppingCart
    {
        Task RemoveCartOfLoggedUserasync(ClaimsPrincipal loggedUser);
    }
}