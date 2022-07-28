namespace Application.Users.Queries.GetUser
{
    public interface IGetUser
    {
        CreateUserModel Execute(int id);
    }
}
