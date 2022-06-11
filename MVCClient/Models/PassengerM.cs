using MVCClient.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class PassengerM
    {
        public int PersonId { get; set; }
        [Required]
        [birthdayValidator]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage ="Please, Enter a valid email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please, Enter a Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="Please enter a Passport ID")] 
        [MaxLength(15)]
        public string PassportID { get; set; }
        public string Status { get;  set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
