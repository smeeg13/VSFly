using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Passenger : Person
    {
        public DateTime CustomerSince { get; set; }
        [Required]
        public string Status { get; set; }

        public virtual List<Booking> Bookings { get; }
    }
}
