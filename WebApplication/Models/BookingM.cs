using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BookingM
    {
        public int FlightNo { get; set; }
        public int PassengerID { get; set; }
    }

}
