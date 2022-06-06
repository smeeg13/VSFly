using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{

    public class Employee : Person
    {
        
        [Required]
        public double Salary { get; set; }

    }
}
