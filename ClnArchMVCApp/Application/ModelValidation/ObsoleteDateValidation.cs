using System.ComponentModel.DataAnnotations;

namespace Application.ModelValidation
{
    public class ObsoleteDateValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

            DateTime dateTime = Convert.ToDateTime(value);
            if (dateTime < DateTime.Now)
            {
                return false;
            }
            return true;
        }

    }
}
