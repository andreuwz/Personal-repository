using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Queries.GetAllFurnituresList
{
    public class GetAllFurnituresQuery : IGetAllFurnituresListQuery
    {
        private readonly IFurnitureRepository furnitureRepository;

        public GetAllFurnituresQuery(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public IEnumerable<FurnitureModel> Execute()
        {
            return furnitureRepository.GetAll()
                .Select(f => new FurnitureModel()
                {
                    Name = f.Name,
                    Description = f.Description,
                    Type = f.Type,
                    Quantity = f.Quantity,
                });
        }
    }
}
