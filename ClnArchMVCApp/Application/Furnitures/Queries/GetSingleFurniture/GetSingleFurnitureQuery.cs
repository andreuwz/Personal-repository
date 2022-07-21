using Application.Interfaces.Persistence;

namespace Application.Furnitures.Queries.GetSingleFurniture
{
    public class GetSingleFurnitureQuery : IGetSingleFurnitureQuery
    {
        public IFurnitureRepository furnitureRepository;
        public GetSingleFurnitureQuery(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public FurnitureModel Execute(int id)
        {
            var item = furnitureRepository.Get(id);

            return new FurnitureModel
            {
                Name = item.Name,
                Id = item.Id,
                Description = item.Description,
                Type = item.Type
            };
        }
    }
}
