using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class PilotM
    {
        public int PersonId { get; set; }
        public int FlightHours { get; set; }

        public IEnumerable<FlightM> Flights { get; set; }
    }
}
