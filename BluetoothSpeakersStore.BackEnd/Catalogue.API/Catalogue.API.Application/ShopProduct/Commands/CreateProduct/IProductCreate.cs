using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.CreateProduct
{
    public interface IProductCreate
    {
        Task<Product> CreateNewProductAsync(CreateProductModel createItem, ClaimsPrincipal loggedUser);
    }
}