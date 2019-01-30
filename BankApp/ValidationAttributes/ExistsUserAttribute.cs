using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.ValidationAttributes
{
    public class ExistsUserAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "There is no user in DB with id {0}.";
        public ExistsUserAttribute() : base(DefaultErrorMessage)
        {

        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (!(value is int))
                return ValidationResult.Success;

            var userService = validationContext.GetService<IUserService>();
            if (userService.Exists((int)value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    FormatErrorMessage(value.ToString()));
            }

        }
    }
}
