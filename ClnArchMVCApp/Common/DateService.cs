namespace Common
{
    public class DateService : IDateService
    {
        public DateTime GetDate() 
        {
            return DateTime.UtcNow;
        }
    }
}