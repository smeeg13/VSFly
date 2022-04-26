using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Person
    {
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }

        public string GivenName { get; set; }
        [Key]
        public int PersonId { get; set; }
        public string Surname { get; set; }

    }
}
