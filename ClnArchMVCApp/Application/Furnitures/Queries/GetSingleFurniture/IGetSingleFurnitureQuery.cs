
namespace Application.Furnitures.Queries.GetSingleFurniture
{
    public interface IGetSingleFurnitureQuery
    {
        FurnitureModel Execute(int id);
    }
}
