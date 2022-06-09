using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MVCClient.Models;
using MVCClient.Validators;

namespace MVCClient.Models
{
    public class PilotAdminM
    {
        public int PersonId { get; set; }
        public int? FlightHours { get; set; }

        public IEnumerable<FlightAdminM> FlightsToPilot { get; set; }
        public IEnumerable<FlightAdminM> FlightsToCoPilot { get; set; }

        [Required(ErrorMessage = "Please enter a Birthday date")]
        [birthdayValidator]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Please, Enter a valid email")]
        [EmailAddress] 
        public string Email { get; set; }
        [Required(ErrorMessage = "Please, Enter a Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter a Passport ID")]
        [MaxLength(15)]
        public string PassportID { get; set; }
        [Range(0, 100000000)]
        public double Salary { get; set; }
    }
}
