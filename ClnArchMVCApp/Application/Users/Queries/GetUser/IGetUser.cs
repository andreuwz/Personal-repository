namespace Application.Users.Queries.GetUser
{
    public interface IGetUser
    {
        UserModel Execute(int id);
    }
}
