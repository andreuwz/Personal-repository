using AutoMapper;
using Cart.API.Application.Exceptions;
using Cart.API.Application.Validations;
using Cart.API.Common;
using Cart.API.Domain;
using Cart.API.DTO.Request;
using Cart.API.DTO.Response;
using Cart.API.Persistence.Repository;
using Cart.API.Web.AsyncMessageBusServices.PublishMessages;
using Cart.API.Web.AsyncMessageBusServices.Rpc;
using System.Security.Claims;
using System.Text.Json;

namespace Cart.API.Application.Commands.CheckoutCart
{
    internal class CheckoutShoppingCart : ICheckoutShoppingCart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartValidations cartValidations;
        private readonly IPublishNewMessage publishNewMessage;
        private readonly IMapper mapper;
        private readonly RpcClient rpcClient;
        private string rpcProductResponse;

        public string RpcProductResponse { get => rpcProductResponse; private set => rpcProductResponse = value; }

        public CheckoutShoppingCart(ICartRepository cartRepository,
            ICartValidations cartValidations, IPublishNewMessage publishNewMessage, IMapper mapper, RpcClient rpcClient)
        {
            this.cartRepository = cartRepository;
            this.cartValidations = cartValidations;
            this.publishNewMessage = publishNewMessage;
            this.mapper = mapper;
            this.rpcClient = rpcClient;
        }

        public async Task ExecutePaymentAsync(ClaimsPrincipal loggedUser)
        {
            double loggedUserBalance;
            Guid loggedUserId;
            ExtractLoggedUserClaims(loggedUser, out loggedUserBalance, out loggedUserId);

            await cartValidations.EnsureUserHasCartAsync(loggedUserId);
            var loggedUserCart = await cartRepository.GetCartByCreatorIdAsync(loggedUserId);

            cartValidations.EnsureCartHasPositiveTotalSum(loggedUserCart);
            cartValidations.EnsureUserBalanceCoversCartSum(loggedUserBalance, loggedUserCart.TotalSum);

            await PublishProductWantedQuantities(loggedUserCart);
            DetermineResponseEventType(RpcProductResponse);

            PublishUpdatedUserBalance(loggedUserCart, loggedUserId, loggedUserBalance);
            await cartRepository.RemoveCartAsync(loggedUserCart);
        }

        private void ExtractLoggedUserClaims(ClaimsPrincipal loggedUser, out double loggedUserBalance, out Guid loggedUserId)
        {
            var loggedUserBalanceClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Balance");
            loggedUserBalance = double.Parse(loggedUserBalanceClaim.Value);
            var loggedUserIdClaim = loggedUser.Claims.FirstOrDefault(claim => claim.Type == "Id");
            loggedUserId = new Guid(loggedUserIdClaim.Value);
        }

        private async Task PublishProductWantedQuantities(ShoppingCart loggedUserCart)
        {
            var productsLIst = mapper.Map<PublishListProductsForPurchaseModel>(loggedUserCart);
            var productQuantityRequest = JsonSerializer.Serialize(productsLIst);
            RpcProductResponse = await rpcClient.CallAsync(productQuantityRequest);
        }

        private void DetermineResponseEventType(string rpcProductResponse)
        {
            var eventType = JsonSerializer.Deserialize<EventTypeModel>(rpcProductResponse);

            if (eventType.EventType == AppConstants.eventTypeSentDisapprovedProductQuantities)
            {
                var disapprovedProduct = JsonSerializer.Deserialize<PublishedProductModel>(rpcProductResponse);
                throw new UnsufficientProductQuantitiesException(AppConstants.ProductQuantitesNotSuffucient(disapprovedProduct.Name, disapprovedProduct.Quantity));
            }
        }

        private void PublishUpdatedUserBalance(ShoppingCart loggedUserCart, Guid loggedUserId, double loggedUserBalance)
        {
            var updatedUserBalance = new PublishUpdatedUserBalance()
            {
                Id = loggedUserId,
                Balance = loggedUserBalance - loggedUserCart.TotalSum
            };

            var balanceMessage = JsonSerializer.Serialize(updatedUserBalance);
            publishNewMessage.PublishMessage(balanceMessage);
        }
    }
}
