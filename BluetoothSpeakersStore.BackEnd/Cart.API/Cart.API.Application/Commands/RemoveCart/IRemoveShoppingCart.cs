namespace Cart.API.Application.Commands.RemoveCart
{
    public interface IRemoveShoppingCart
    {
        Task RemoveCartByCartIdAsync(Guid id);
    }
}