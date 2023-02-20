using Cart.API.Application.Exceptions;
using Cart.API.Application.Validations;
using Cart.API.Common;
using Cart.API.DTO.Request;
using System.Text.Json;

namespace Cart.API.Web.AsyncMessageBusServices.Rpc
{
    public class RpcHelperService : IRpcHelperService
    {
        private readonly ICartValidations cartValidations;

        public RpcHelperService(ICartValidations cartValidations)
        {
            this.cartValidations = cartValidations;
        }

        public void DetermineResponseEventType(string rpcProductResponse)
        {
            var eventType = JsonSerializer.Deserialize<EventTypeModel>(rpcProductResponse);

            if (eventType.EventType == AppConstants.eventTypeSentDisapprovedProductQuantities)
            {
                var disapprovedProduct = JsonSerializer.Deserialize<PublishedProductModel>(rpcProductResponse);
                throw new UnsufficientProductQuantitiesException(AppConstants.ProductQuantitesNotSuffucient(disapprovedProduct.Name, disapprovedProduct.Quantity));
            }
        }

        public void ValidateBuyerBalance(double balance, double totalSum)
        {
            cartValidations.EnsureUserBalanceCoversCartSum(balance, totalSum);
        }
    }
}
