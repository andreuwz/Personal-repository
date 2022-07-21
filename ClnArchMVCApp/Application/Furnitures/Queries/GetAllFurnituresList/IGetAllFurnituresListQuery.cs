using Domain.Furnitures;

namespace Application.Furnitures.Queries.GetAllFurnituresList
{
    public interface IGetAllFurnituresListQuery
    {
        public IEnumerable<Furniture> GetAllFurnitures();
    }
}
