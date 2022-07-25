using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Commands.BuyFurniture
{
    public class BuyFurniture : IBuyFurniture
    {
        private readonly IFurnitureRepository furnitureRepository;

        public BuyFurniture(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public Furniture Execute(int id, int quantity)
        {
            var foundModel = furnitureRepository.Get(id);

            if (foundModel.Quantity >= quantity)
            {
                foundModel.Quantity -= quantity;
                furnitureRepository.Update(foundModel);

                return foundModel;
            }

            return null;
        }
    }
}
