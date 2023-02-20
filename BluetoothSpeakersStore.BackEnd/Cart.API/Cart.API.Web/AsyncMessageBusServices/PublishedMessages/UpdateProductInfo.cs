using AutoMapper;
using Cart.API.Application.Validations;
using Cart.API.DTO.Request;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices.PublishedMessages.Contracts;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public class UpdateProductInfo : IUpdateProductInfo
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;
        private readonly ICartValidations cartValidations;

        public UpdateProductInfo(ICartRepository cartRepository, IMapper mapper, ICartValidations cartValidations)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
            this.cartValidations = cartValidations;
        }

        public async Task UpdateProductsInCartAsync(PublishedProductModel productModel)
        {
            var products = await cartRepository.GetProductsListByProductId(productModel.Id);
            cartValidations.EnsureListOfProductsExists(products);

            foreach (var product in products)
            {
                product.Carts.ForEach(cart =>
                {
                    cart.TotalSum -= product.TotalPrice;
                    product.Name = productModel.Name;

                    if (productModel.Quantity > 0)
                    {
                        product.Comment = "In stock";
                        product.Price = productModel.Price;
                        product.TotalPrice = product.Price * product.Quantity;
                        cart.TotalSum += product.TotalPrice;
                    }
                    else
                    {
                        product.Comment = "Out of stock";
                        product.Price = productModel.Price;
                        product.Quantity = 0;
                        product.TotalPrice = 0;
                    }
                });
            }
            
            await cartRepository.SaveChangesAsync();
        }
    }
}
