using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Ticket
    {
        public string FullName { get; set; }
        public int PassengerId { get; set; }

        public int FlightNo { get; set; }
        public double SalePrice { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
