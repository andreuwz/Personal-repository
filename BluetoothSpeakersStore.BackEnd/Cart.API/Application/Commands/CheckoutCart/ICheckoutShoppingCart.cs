using System.Security.Claims;

namespace Cart.API.Application.Commands.CheckoutCart
{
    public interface ICheckoutShoppingCart
    {
        Task ExecutePaymentAsync(ClaimsPrincipal loggedUser);
    }
}