using Cart.API.Dto.Response;
using System.Security.Claims;

namespace Cart.API.Application.Commands.CheckoutCart
{
    public interface ICheckoutShoppingCart
    {
        Task<GetBuyerInfoModel> GetBuyerInformation(ClaimsPrincipal loggedUser);
    }
}