using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infrastructure.CustomAttributes
{
    public class WeberEmailDomainAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value as string;
            if (email != null)
            {
                if (Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@mail\.weber\.edu$"))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("The Email must be a mail.weber.edu domain.");
                }
            }
            return new ValidationResult("Email is required");
        }
    }
}
