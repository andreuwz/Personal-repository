using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Persistance.Repositories
{
    public class FurnitureRepository : Repository<Furniture>, IFurnitureRepository
    {
        public FurnitureRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
