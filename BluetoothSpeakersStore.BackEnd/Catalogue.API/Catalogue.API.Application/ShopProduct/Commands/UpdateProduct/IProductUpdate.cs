using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.UpdateProduct
{
    public interface IProductUpdate
    {
        Task<Product> UpdateItemAsync(Guid id, UpdateProductModel productModel, ClaimsPrincipal loggedUser);
    }
}