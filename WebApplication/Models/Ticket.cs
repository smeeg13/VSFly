using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Ticket
    {
        public string fullName { get; set; }
        public int FlightNo { get; set; }
        public double SalePrice { get; set; }
    }
}
