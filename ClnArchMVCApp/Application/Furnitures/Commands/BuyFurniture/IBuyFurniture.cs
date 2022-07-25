using Domain.Furnitures;

namespace Application.Furnitures.Commands.BuyFurniture
{
    public interface IBuyFurniture
    {
        Furniture Execute(int id, int quantity);
    }
}
