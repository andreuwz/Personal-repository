using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.DTO.Request;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public class UpdatedUserInfo : IUpdatedUserInfo
    {
        private readonly IProductRepository productRepository;
        private readonly IProductValidations productValidations;

        public UpdatedUserInfo(IProductRepository productRepository, IProductValidations productValidations)
        {
            this.productRepository = productRepository;
            this.productValidations = productValidations;
        }

        public async Task UpdateProductsCreatedByUpdatedUser(PublishedUpdatedUserModel updatedUser)
        {
            var userProducts = await productRepository.GetProductsByCreatorId(updatedUser.Id);
            productValidations.EnsureUserHasProducts(userProducts);

            foreach (var product in userProducts)
            {   
                product.CreatorName = updatedUser.UserName;
                product.ModifierName = updatedUser.UserName;
                product.ModifiedAt = DateTime.Now.Date;
            }

            await productRepository.SaveChangesAsync();
        }
    }
}
