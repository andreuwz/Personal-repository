using Application.Interfaces.Persistence;
using Domain.Furnitures;

namespace Application.Users.Commands.UserAddItem
{
    public class UserAddItem : IUserAddItem
    {
        private readonly IUserRepository userRepository;

        private readonly IFurnitureRepository furnitureRepository;

        public UserAddItem(IUserRepository userRepository, IFurnitureRepository furnitureRepository)
        {
            this.userRepository = userRepository;
            this.furnitureRepository = furnitureRepository;
        }

        public Furniture Execute(int furnitureId, int userId)
        {
            var foundItem = furnitureRepository.Get(furnitureId);
            var foundUser = userRepository.Get(userId);

            foundUser.Furnitures.Add(foundItem);
            userRepository.Update(foundUser);

            return foundItem;
        }
    }
}
