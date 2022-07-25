using Application.Interfaces.Persistence;

namespace Application.Furnitures.Queries.GetAllFurnituresListAdmin
{
    public class GetAllFurnituresListAdminQuery : IGetAllFurnituresListAdminQuery
    {
        private readonly IFurnitureRepository furnitureRepository;
        public GetAllFurnituresListAdminQuery(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }
        public IEnumerable<FurnitureModel> Execute()
        {
            return furnitureRepository.GetAll()
                .Select(f => new FurnitureModel()
                {
                    Id = f.Id,  
                    Name = f.Name,
                    Description = f.Description,
                    Type = f.Type,
                    Quantity = f.Quantity,
                });
        }
    }
}
