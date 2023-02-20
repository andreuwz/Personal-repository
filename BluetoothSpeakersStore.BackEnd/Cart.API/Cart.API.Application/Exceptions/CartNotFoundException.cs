namespace Cart.API.Application.Exceptions
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string message) : base(message)
        {

        }
    }
}
