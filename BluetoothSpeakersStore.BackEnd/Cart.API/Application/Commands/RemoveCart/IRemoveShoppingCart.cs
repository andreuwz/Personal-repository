namespace Cart.API.Application.Commands.RemoveCart
{
    public interface IRemoveShoppingCart
    {
        Task RemoveCartByUserIdAsync(Guid id);
    }
}