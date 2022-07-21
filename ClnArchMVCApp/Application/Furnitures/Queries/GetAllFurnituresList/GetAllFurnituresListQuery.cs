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

        public IEnumerable<FurnitureModel> GetAllFurnitures()
        {
            return furnitureRepository.GetAll()
                .Select(f => new FurnitureModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    Type = f.Type

                });
        }
    }
}
