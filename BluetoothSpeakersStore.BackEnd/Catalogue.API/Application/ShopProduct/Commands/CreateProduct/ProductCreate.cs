using AutoMapper;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using Catalogue.API.Persistence.Repository;
using System.Security.Claims;

namespace Catalogue.API.Application.ShopProduct.Commands.CreateProduct
{
    internal class ProductCreate : IProductCreate
    {
        private readonly IProductRepository itemRepository;
        private readonly IMapper mapper;

        public ProductCreate(IProductRepository itemRepository, IMapper mapper)
        {
            this.itemRepository = itemRepository;
            this.mapper = mapper;
        }

        public async Task<Product> CreateNewProductAsync(CreateProductModel createItem, ClaimsPrincipal loggedUser)
        {
            var newItem = mapper.Map<Product>(createItem);
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var loggedUserUsernameClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

            newItem.CreatorName = loggedUserUsernameClaim.Value;
            newItem.CreatorId = loggedUserIdClaim.Value;
            newItem.CreatedAt = DateTime.Now.Date;
            newItem.ModifierName = loggedUserUsernameClaim.Value;
            newItem.ModifiedAt = DateTime.Now.Date; 

            await itemRepository.CreateNewItemAsync(newItem);
            return newItem;
        }
    }
}
