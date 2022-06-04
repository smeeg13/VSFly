using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class BookFlight
    {

        public int FlightNo { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }

        public int NbPassengers { get; set; }
        public PassengerM Passenger { get; set; }
        public BookingM Booking { get; set; }

        public List<PassengerM> Passengers { get; set; }

        public List<BookingM> Bookings { get; set; }

        public BookFlight()
        {
            NbPassengers = 1;
        }
    }
}
