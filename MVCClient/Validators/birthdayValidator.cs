﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Validators
{
    public class birthdayValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date of Birth should be completed");

            var dateTime = (DateTime)value;
            if (dateTime > DateTime.Now)
                return new ValidationResult("Date of Birth can not be greater todays date");


            return ValidationResult.Success;
        }
    }
}
