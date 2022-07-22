using Domain.Furnitures;

namespace Application.Furnitures.Commands.RemoveFurniture
{
    public interface IRemoveFurniture
    {
        void Execute(Furniture furniture);
    }
}
