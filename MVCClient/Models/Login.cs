using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Login
    {

        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Passport ID cannot be empty")]
        public string PassportID { get; set; }

        public Boolean IsPilot { get; set; }
    }
}
