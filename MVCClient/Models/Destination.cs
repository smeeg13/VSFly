using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Destination
    {
        public string DestinationName { get; set; }

        public List<FlightAdminM> Flights { get; set; }
        public List<Ticket> TicketsSold { get; set; }

        public double SumSales { get; set; }
        public double AvgSales { get; set; }
      
    }
}


