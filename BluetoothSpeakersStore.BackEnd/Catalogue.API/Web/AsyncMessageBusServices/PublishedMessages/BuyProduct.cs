using AutoMapper;
using Catalogue.API.Application.ShopProduct.Queries.GetAllProductsAdmin;
using Catalogue.API.Application.ShopProduct.Validations;
using Catalogue.API.Common;
using Catalogue.API.DTO.Response;
using Catalogue.API.Persistence.Repository;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages
{
    internal class BuyProduct : IBuyProduct
    {
        private readonly IProductRepository productRepository;
        private readonly IProductValidations productValidations;
        private readonly IMapper mapper;
        private readonly IGetAllProductsAsAdmin getAllProductsAsAdmin;
        public PublishProductToCartModel ProductQuantityEvaluationModel { get; private set; }

        public BuyProduct(IProductRepository productRepository, IProductValidations productValidations,
            IMapper mapper, IGetAllProductsAsAdmin getAllProductsAsAdmin)
        {
            this.productRepository = productRepository;
            this.productValidations = productValidations;
            this.mapper = mapper;
            this.getAllProductsAsAdmin = getAllProductsAsAdmin;
        }

        public async Task BuyProductAsync(SessionState sessionState)
        {
            var productsForBuy = sessionState.ProductsListForBuy;
            var availableProducts = await getAllProductsAsAdmin.GetAllProductsAsync();

            var matchingAvailableProducts = productsForBuy
                .Where(prod => availableProducts
                    .Any(dest => prod.Id == dest.Id && dest.Quantity >= prod.Quantity))
                        .ToList();

            var matchingUnavailableProducts = productsForBuy
                .Where(prod => availableProducts
                    .Any(dest => prod.Id == dest.Id && dest.Quantity < prod.Quantity))
                        .ToList();

            if (matchingUnavailableProducts.Count != 0)
            {
                var product = matchingUnavailableProducts.FirstOrDefault();
                var getActualProduct = await productRepository.GetShoppingProductAsync(product.Id);

                var sendDisapprovedQuantitiesProduct = mapper.Map<PublishProductToCartModel>(getActualProduct);
                sendDisapprovedQuantitiesProduct.EventType = AppConstants.eventTypeSendDisapprovedProductQuantities;

                ProductQuantityEvaluationModel = sendDisapprovedQuantitiesProduct;
            }

            else
            {
                foreach (var product in matchingAvailableProducts)
                {
                    await productValidations.EnsureProductExistsAsync(product.Id);
                    var getActualProduct = await productRepository.GetShoppingProductAsync(product.Id);

                    getActualProduct.Quantity -= product.Quantity;
                    await productRepository.SaveChangesAsync();

                    var approvedProductQuantities = new PublishProductToCartModel()
                    {
                        EventType = AppConstants.eventTypeApprovedQuantities
                    };
                    ProductQuantityEvaluationModel = approvedProductQuantities;
                }
            }
        }
    }
}
