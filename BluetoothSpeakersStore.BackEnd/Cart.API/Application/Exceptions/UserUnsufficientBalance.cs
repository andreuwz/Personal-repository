namespace Cart.API.Application.Exceptions
{
    public class UserUnsufficientBalance : Exception
    {
        public UserUnsufficientBalance(string message) : base(message)
        {

        }
    }
}
