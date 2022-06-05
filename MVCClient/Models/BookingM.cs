using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class BookingM
    {
        public int FlightNo { get; set; }
        public int PassengerID { get; set; }
        public double SalePrice { get; set; }

        public FlightM Flight { get; set; }
        public PassengerM Passenger { get; set;  }
    }

}
