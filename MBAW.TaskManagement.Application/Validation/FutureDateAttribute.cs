using System.ComponentModel.DataAnnotations;

namespace MBAW.TaskManagement.Application.Validation
{

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date && date < DateTime.Now)
            {
                return new ValidationResult("Due date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }

}
