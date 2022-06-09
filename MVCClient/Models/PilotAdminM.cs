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

        [birthdayValidator] 
        public DateTime Birthday { get; set; }
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)] 
        public string PassportID { get; set; }
        public double Salary { get; set; }
    }
}
