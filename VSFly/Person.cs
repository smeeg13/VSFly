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
       [Key]
        public int PersonId { get; set; } 
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }

    }
}
