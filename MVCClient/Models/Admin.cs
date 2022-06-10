using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Admin
    {
        public IEnumerable<FlightAdminM> Flights { get; set; }
        public IEnumerable<PassengerM> Passengers { get; set; }
        public IEnumerable<BookingM> Bookings { get; set; }
        public IEnumerable<PilotAdminM> Pilots { get; set; }

        public string searchDestination { get; set; }
        public string searchDeparture { get; set; }

        public string searchFlightNo { get; set; }
        public Boolean onlyAvailableFlights { get; set; }


        public SelectList flightsNo { get; set; }

    }
}
