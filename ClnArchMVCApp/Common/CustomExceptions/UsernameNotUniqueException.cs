using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace Common.CustomExceptions
{
    public class UsernameNotUniqueException : Exception
    {
        public UsernameNotUniqueException()
        {
        }

        public UsernameNotUniqueException(string? message) : base(message)
        {
        }

        public UsernameNotUniqueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UsernameNotUniqueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
