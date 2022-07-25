using Domain.Furnitures;

namespace Application.Users.Commands.UserAddItem
{
    public interface IUserAddItem
    {
        Furniture Execute(int furnitureId, int userId);
    }
}
