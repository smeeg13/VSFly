using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class PilotAdminM
    {
        public int PersonId { get; set; }
        public int? FlightHours { get; set; }

        public IEnumerable<FlightAdminM> Flights { get; set; }
      
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public string PassportID { get; set; }
        public double Salary { get; set; }
    }
}
