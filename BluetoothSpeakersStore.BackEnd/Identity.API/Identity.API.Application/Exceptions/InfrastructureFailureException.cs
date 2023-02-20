namespace Identity.API.Application.Exceptions
{
    public class InfrastructureFailureException : Exception
    {
        public InfrastructureFailureException(string message) : base(message)
        {

        }
    }
}
