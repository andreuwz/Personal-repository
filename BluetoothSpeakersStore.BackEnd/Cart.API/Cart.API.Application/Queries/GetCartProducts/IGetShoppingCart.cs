using Cart.API.DTO.Response;

namespace Cart.API.Application.Queries.GetCartProducts
{
    public interface IGetShoppingCart
    {
        Task<IEnumerable<GetCartProductsModel>> GetCartByIdAsync(Guid id);
    }
}