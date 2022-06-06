using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Destination
    {
        public Destination(string destinationName)
        {
            DestinationName = destinationName;
            Flights = new List<FlightAdminM>();
        }

        public string DestinationName { get; set; }

        public List<FlightAdminM> Flights { get; set; }
        public double SumSales { get; set; }
        public double AvgSales { get; set; }

    }
}
