using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankApp.BLL;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LessThanBalanceAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Lack of money to make the operation";
        public string OtherPropertyUserId { get; private set; }

        public LessThanBalanceAttribute(string otherPropertyUserId)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherPropertyUserId))
            {
                throw new ArgumentNullException("otherPropertyUserId");
            }

            OtherPropertyUserId = otherPropertyUserId;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (!(value is double))
                return ValidationResult.Success;

            var otherPropertyUserId = validationContext.ObjectInstance.GetType()
                .GetProperty(OtherPropertyUserId);

            var otherPropertyUserIdValue = otherPropertyUserId
                .GetValue(validationContext.ObjectInstance, null);

            var userService = validationContext.GetService<IUserService>();
            var user = userService.GetUserFullInfoById((int)otherPropertyUserIdValue);

            if (user == null || (double)value <= user.Balance)
                return ValidationResult.Success;

            return new ValidationResult(DefaultErrorMessage);
        }
    }
}
