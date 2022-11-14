namespace Identity.API.Application.Exceptions
{
    public class ProhibitedAdminAccountActionException : Exception
    {
        public ProhibitedAdminAccountActionException(string message) : base(message)
        {

        }
    }
}
