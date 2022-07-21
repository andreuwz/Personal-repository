using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Furnitures.Commands.AddFurniture.FurnitureFactory
{
    public class FurnitureFactory : IFurnitureFactory
    {
        private readonly IFurnitureRepository furnitureRepository;

        public FurnitureFactory(IFurnitureRepository furnitureRepository)
        {
            this.furnitureRepository = furnitureRepository;
        }

        public Furniture Execute(string name, string type, string description)
        {

            Furniture newItem = new Furniture()
            {
                Name = name,
                Type = type,    
                Description = description
            };
            furnitureRepository.Add(newItem);
            return newItem;
        }
    }
}
