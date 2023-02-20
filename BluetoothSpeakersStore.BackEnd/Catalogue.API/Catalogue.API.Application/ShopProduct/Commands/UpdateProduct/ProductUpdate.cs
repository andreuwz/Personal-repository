using AutoMapper;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using Catalogue.API.Persistence.Repository;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.UpdateProduct
{
    public class ProductUpdate : IProductUpdate
    {
        private readonly IMapper mapper;
        private readonly IProductRepository itemRepository;
        private readonly IProductValidations productValidations;

        public ProductUpdate(IMapper mapper, IProductRepository itemRepository, IProductValidations productValidations)
        {
            this.mapper = mapper;
            this.itemRepository = itemRepository;
            this.productValidations = productValidations;
        }

        public async Task<Product> UpdateItemAsync(Guid id, UpdateProductModel productModel, ClaimsPrincipal loggedUser)
        {
            await productValidations.EnsureProductExistsAsync(id);
            var loggedUserUsernameClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

            var productForUpdate = await itemRepository.GetShoppingProductAsync(id);

            mapper.Map(productModel, productForUpdate);
            productForUpdate.ModifierName = loggedUserUsernameClaim.Value;
            productForUpdate.ModifiedAt = DateTime.Now.Date;

            await itemRepository.SaveChangesAsync();
            return productForUpdate;
        }
    }
}
