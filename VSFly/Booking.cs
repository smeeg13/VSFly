using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Booking
    {
        [Key]
        public int FlightNo { get; set; }
        public int PassengerID { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
    }

}
