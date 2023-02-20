namespace Catalogue.API.Application.Exceptions
{
    public class UnsufficientProductQuantity : Exception 
    {
        public UnsufficientProductQuantity(string message) : base(message)
        {

        }
    }
}
