using Cart.API.DTO.Response;

namespace Cart.API.Application.Queries.GetUserCart
{
    public interface IGetCartOfUser
    {
        Task<GetUserCartModel> GetCartByUserIdAsync(Guid userId);
    }
}