using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Queries.GetAllFurnituresList
{
    public class GetAllFurnituresQuery : IGetAllFurnituresListQuery
    {

        private readonly IUserRepository userRepository;

        public GetAllFurnituresQuery(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<Furniture> GetAllFurnitures()
        {
            throw new NotImplementedException();
        }
    }
}
