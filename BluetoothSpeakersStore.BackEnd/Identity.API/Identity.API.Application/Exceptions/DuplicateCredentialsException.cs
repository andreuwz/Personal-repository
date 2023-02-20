namespace Identity.API.Application.Exceptions
{
    public class DuplicateCredentialsException : Exception
    {
        public DuplicateCredentialsException(string message) : base(message)
        {

        }
    }
}
