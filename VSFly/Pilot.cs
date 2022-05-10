using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Pilot : Employee
    {
        public int? FlightHours { get; set; }

        public string FlightSchool { get; set; }
        public DateTime LicenseDate { get; set; }
        public virtual List<Flight> Flights { get; }

    }
}
