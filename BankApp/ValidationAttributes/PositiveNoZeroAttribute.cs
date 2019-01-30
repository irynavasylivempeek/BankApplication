using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PositiveNoZeroAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "The {0} must be positive number.";
        public PositiveNoZeroAttribute() : base(DefaultErrorMessage)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var displayName = validationContext.DisplayName;
            if (!(value is double))
            {
                return ValidationResult.Success;
            }
            if (int.TryParse(value.ToString(), out int number))
            {
                if (number > 0)
                    return ValidationResult.Success;
            }
            return new ValidationResult(FormatErrorMessage(displayName));

        }
    }
}
