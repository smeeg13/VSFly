using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Validators
{
    public class flightDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date of flight should be completed");

            var dateTime = (DateTime)value;
            if (dateTime < DateTime.Now)
                return new ValidationResult("Date of flight can not be before todays date");
            if (dateTime > DateTime.Now.AddYears(20))
                return new ValidationResult("Date of flight can not be later than 20 years from now");

            return ValidationResult.Success;
        }
    }
}
