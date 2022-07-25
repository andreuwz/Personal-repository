using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Commands.RemoveFurniture
{
    public class RemoveFurniture : IRemoveFurniture
    {
        private readonly IFurnitureRepository furnitureRepository;

        public RemoveFurniture(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public void Execute(int id)
        {
            var foundModel = furnitureRepository.Get(id);
            furnitureRepository.Delete(foundModel);
        }
    }
}
