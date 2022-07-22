using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Commands.UpdateFurniture
{
    public class UpdateFurniture : IUpdateFurniture
    {
        private readonly IFurnitureRepository furnitureRepository;

        public UpdateFurniture(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public void Execute(Furniture furniture)
        {
            var foundModel = furnitureRepository.Get(furniture.Id);

            foundModel.Name = furniture.Name;
            foundModel.Description = furniture.Description;
            foundModel.Type = furniture.Type;
            foundModel.Quantity = furniture.Quantity;

            furnitureRepository.Update(foundModel);
        }
    }
}
