using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Login
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public int PersonID { get; set; }
    }
}
