using Catalogue.API.DTO.Response;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct
{
    public interface IAddProductToCart
    {
        Task<PublishProductToCartModel> AddProductToCartAsync(Guid id, int quantity, ClaimsPrincipal loggedUser);
    }
}