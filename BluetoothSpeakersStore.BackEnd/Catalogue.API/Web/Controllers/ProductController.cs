using Catalogue.API.Application.ShopProduct.Commands.AddToCartProduct;
using Catalogue.API.Application.ShopProduct.Commands.CreateProduct;
using Catalogue.API.Application.ShopProduct.Commands.RemoveProduct;
using Catalogue.API.Application.ShopProduct.Commands.UpdateProduct;
using Catalogue.API.Application.ShopProduct.Queries.GetAllProducts;
using Catalogue.API.Application.ShopProduct.Queries.GetAllProductsAdmin;
using Catalogue.API.Application.ShopProduct.Queries.GetProduct;
using Catalogue.API.Application.ShopProduct.Queries.GetProductAdmin;
using Catalogue.API.Common;
using Catalogue.API.DTO.Request;
using Catalogue.API.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCreate createNewItem;
        private readonly IProductRemove removeItem;
        private readonly IProductUpdate updateItem;
        private readonly IGetAllProducts getAllProducts;
        private readonly IGetAllProductsAsAdmin getAllProductsAsAdmin;
        private readonly IGetProduct getProduct;
        private readonly IGetProductAsAdmin getProductAsAdmin;
        private readonly IAddProductToCart addProductToCart;

        public ProductController(IProductCreate createNewItem, IProductRemove removeItem, IProductUpdate updateItem, IGetAllProducts getAllProducts,
            IGetAllProductsAsAdmin getAllProductsAsAdmin, IGetProduct getProduct, IGetProductAsAdmin getProductAsAdmin,
            IAddProductToCart addProductToCart)
        {
            this.createNewItem = createNewItem;
            this.removeItem = removeItem;
            this.updateItem = updateItem;
            this.getAllProducts = getAllProducts;
            this.getAllProductsAsAdmin = getAllProductsAsAdmin;
            this.getProduct = getProduct;
            this.getProductAsAdmin = getProductAsAdmin;
            this.addProductToCart = addProductToCart;
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewProductAsync(CreateProductModel newModel)
        {
            if (ModelState.IsValid)
            {
                await createNewItem.CreateNewProductAsync(newModel, User);
                return Created(nameof(HttpPostAttribute),AppConstants.SerializeSingleMessage(AppConstants.createdProduct));
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));

        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, UpdateProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                await updateItem.UpdateItemAsync(id, productModel, User);

                return Ok(AppConstants.SerializeSingleMessage(AppConstants.updatedProduct));
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProductAsync(Guid id)
        {
            await removeItem.RemoveProductByIdAsync(id);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.removedProduct));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductModel>>> GetAllProductsAsync()
        {
            return Ok(await getAllProducts.GetAllProductsAsync());
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpGet]
        [Route("Admin")]
        public async Task<ActionResult<IEnumerable<GetProductModelAdmin>>> GetAllProductsAdminAsync()
        {
            return Ok(await getAllProductsAsAdmin.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetProductModel>>> GetProductByIdAsync(Guid id)
        {
            return Ok(await getProduct.GetProductByIdAsync(id));
        }

        [Authorize(Roles = "Administrator,MasterAdmin")]
        [HttpGet]
        [Route("Admin/{id}")]
        public async Task<ActionResult<IEnumerable<GetProductModelAdmin>>> GetProductByIdAdminAsync(Guid id)
        {
            return Ok(await getProductAsAdmin.GetProductByIdAsync(id));
        }

        [Authorize]
        [HttpPost]
        [Route("AddToCart/{productId}/Quantity/{quantity}")]
        public async Task<ActionResult> AddProductToCartAsync(Guid productId, int quantity)
        {
            await addProductToCart.AddProductToCartAsync(productId, quantity, User);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.sentProduct));
        }
    }
}
