using Domain.Furnitures;

namespace Application.Furnitures.Commands.AddFurniture.FurnitureFactory
{
    public interface IFurnitureFactory
    {
        Furniture Execute(string name, string type, string description);
    }
}
