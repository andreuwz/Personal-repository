using Application.Interfaces.Persistence;

namespace Application.Furnitures.Commands.RemoveFurniture
{
    public class RemoveFurniture : IRemoveFurniture
    {
        private readonly IFurnitureRepository furnitureRepository;
        public RemoveFurniture(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }
        public void Execute(FurnitureModel furnitureModel)
        {
            var foundModel = furnitureRepository.Get(furnitureModel.Id);
            furnitureRepository.Delete(foundModel);
        }
    }
}
