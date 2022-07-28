using System.ComponentModel.DataAnnotations;

namespace Application.ModelValidation
{
    public class ObsoleteDateValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

            DateTime dateTime = Convert.ToDateTime(value).Date;
            if (dateTime < DateTime.Now.Date)
            {
                return false;
            }
            return true;
        }

    }
}
