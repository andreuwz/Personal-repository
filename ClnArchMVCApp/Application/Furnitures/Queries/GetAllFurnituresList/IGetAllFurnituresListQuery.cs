using Domain.Furnitures;

namespace Application.Furnitures.Queries.GetAllFurnituresList
{
    public interface IGetAllFurnituresListQuery
    {
        public IEnumerable<FurnitureModel> Execute();
    }
}
