using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct
{
    public interface IAddProductToCart
    {
        Task<bool> AddProductToCartAsync(Guid id, int quantity, ClaimsPrincipal loggedUser);
    }
}