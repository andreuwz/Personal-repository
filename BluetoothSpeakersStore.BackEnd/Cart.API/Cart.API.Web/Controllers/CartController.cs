using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cart.API.Application.Commands.CheckoutCart;
using Cart.API.Application.Commands.RemoveCart;
using Cart.API.Application.Commands.RemoveLoggedUserCart;
using Cart.API.Application.Queries.GetCartProducts;
using Cart.API.Application.Queries.GetCurrentUserCartProducts;
using Cart.API.Common;
using Cart.API.DTO.Response;
using Cart.API.Application.Commands.RemoveProductFromCart;
using Cart.API.Application.Queries.GetCurrentUserCart;
using Cart.API.Application.Queries.GetUserCart;
using Cart.API.Web.AsyncMessageBusServices.Rpc;
using Cart.API.Web.AsyncMessageBusServices.PublishMessages;

namespace Cart.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IGetLoggedUserProductsInCart getLoggedUserCartProducts;
        private readonly IGetShoppingCart getCartProducts;
        private readonly IRemoveLoggedUserShoppingCart removeLoggedUserShoppingCart;
        private readonly IRemoveShoppingCart removeShoppingCart;
        private readonly ICheckoutShoppingCart checkoutShoppingCart;
        private readonly IRemoveProductInCart removeProductInCart;
        private readonly IGetLoggedUserCart getLoggedUserCart;
        private readonly IGetCartOfUser getCartByUserId;
        private readonly IUserRpcClient userRpcClient;
        private readonly IProductRpcClient productRpcClient;
        private readonly IPublishNewMessage publishNewMessage;
        private readonly IRpcHelperService rpcHelperService;

        public CartController(IGetLoggedUserProductsInCart getLoggedUserCartProducts,
            IGetShoppingCart getCartProducts, IRemoveLoggedUserShoppingCart removeLoggedUserShoppingCart,
            IRemoveShoppingCart removeShoppingCart, ICheckoutShoppingCart checkoutShoppingCart,
            IRemoveProductInCart removeProductInCart, IGetLoggedUserCart getLoggedUserCart, IGetCartOfUser getCartByUserId,
            IUserRpcClient userRpcClient, IProductRpcClient productRpcClient, IPublishNewMessage publishNewMessage, IRpcHelperService rpcHelperService)
        {
            this.getLoggedUserCartProducts = getLoggedUserCartProducts;
            this.getCartProducts = getCartProducts;
            this.removeLoggedUserShoppingCart = removeLoggedUserShoppingCart;
            this.removeShoppingCart = removeShoppingCart;
            this.checkoutShoppingCart = checkoutShoppingCart;
            this.removeProductInCart = removeProductInCart;
            this.getLoggedUserCart = getLoggedUserCart;
            this.getCartByUserId = getCartByUserId;
            this.userRpcClient = userRpcClient;
            this.productRpcClient = productRpcClient;
            this.publishNewMessage = publishNewMessage;
            this.rpcHelperService = rpcHelperService;
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpGet("Admin/ProductsInCart/{id}")]
        public async Task<ActionResult<IEnumerable<GetCartProductsModel>>> GetProductsInCartByIdAsync(Guid id)
        {
            return Ok(await getCartProducts.GetCartByIdAsync(id));
        }

        [Authorize]
        [HttpGet("LoggedUser/ProductsInCart")]
        public async Task<ActionResult<IEnumerable<GetCartProductsModel>>> GetLoggedUserCartProductsAsync()
        {
            return Ok(await getLoggedUserCartProducts.GetLoggedUserCartProductsAsync(User));
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpDelete("Admin/{id}")]
        public async Task<IActionResult> RemoveCartByIdAsync(Guid id)
        {
            await removeShoppingCart.RemoveCartByCartIdAsync(id);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.removeCart));
        }

        [Authorize]
        [HttpDelete("LoggedUser")]
        public async Task<IActionResult> RemoveCartOfLoggedUserAsync()
        {
            await removeLoggedUserShoppingCart.RemoveCartOfLoggedUserasync(User);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.removeCartOfLoggedUser));
        }

        [Authorize]
        [HttpPost("LoggedUser/CartCheckout")]
        public async Task<IActionResult> CartCheckout()
        {
            var buyerInfo = await checkoutShoppingCart.GetBuyerInformation(User);
            var userBalance = await userRpcClient.AcquireUserBalance(buyerInfo.BuyerId);

            rpcHelperService.ValidateBuyerBalance(userBalance, buyerInfo.BuyerCart.TotalSum);

            var productResponse = await productRpcClient.AcquireProductWantedQuantities(buyerInfo.BuyerCart);
            rpcHelperService.DetermineResponseEventType(productResponse);

            publishNewMessage.PublishUpdatedUserBalance(buyerInfo.BuyerCart, buyerInfo.BuyerId, userBalance);
            await removeShoppingCart.RemoveCartByCartIdAsync(buyerInfo.BuyerCart.CartId);

            return Ok(AppConstants.SerializeSingleMessage(AppConstants.successfullCheckout));
        }

        [Authorize]
        [HttpDelete("LoggedUser/ProductInCart/{id}")]
        public async Task<IActionResult> RemoveProductFromLoggedUserCart(Guid id)
        {
            await removeProductInCart.RemoveProductFromLoggedUserCartAsync(id, User);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.inCartProductDeleted));
        }

        [Authorize]
        [HttpGet("LoggedUser")]
        public async Task<IActionResult> GetLoggedUserCart()
        {
            return Ok(await getLoggedUserCart.GetLoggedUserCartAsync(User));
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpGet("Admin/{id}")]
        public async Task<IActionResult> GetCartByUserId(Guid id)
        {
            return Ok(await getCartByUserId.GetCartByUserIdAsync(id));
        }
    }
}
