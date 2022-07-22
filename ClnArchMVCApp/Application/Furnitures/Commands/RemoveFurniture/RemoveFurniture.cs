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

        public void Execute(Furniture furniture)
        {
            var foundModel = furnitureRepository.Get(furniture.Id);
            furnitureRepository.Delete(foundModel);
        }
    }
}
