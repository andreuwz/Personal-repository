namespace Cart.API.Application.Exceptions
{
    public class UserDuplicateCartsException : Exception
    {
        public UserDuplicateCartsException(string message) : base(message)
        {

        }
    }
}
