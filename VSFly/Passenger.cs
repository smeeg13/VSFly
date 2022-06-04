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
        
        public string Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; }
    }
}
