using Domain.Furnitures;

namespace Application.Furnitures.Commands.UpdateFurniture
{
    public interface IUpdateFurniture
    {
        void Execute(Furniture furniture);
    }
}
