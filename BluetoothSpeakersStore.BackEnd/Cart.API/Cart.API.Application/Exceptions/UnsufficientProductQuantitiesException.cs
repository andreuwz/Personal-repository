namespace Cart.API.Application.Exceptions
{
    public class UnsufficientProductQuantitiesException : Exception
    {
        public UnsufficientProductQuantitiesException(string message) : base(message)
        {

        }
    }
}
