using MVCClient.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Please, Enter your Name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please, Enter your Passport ID")]
        [MaxLength(15)] 
        public string PassportID { get; set; }
        [Required(ErrorMessage = "Please, Enter your email")]
        [EmailAddress(ErrorMessage ="Email entered not Valid")]
        public string Email { get; set; }
        [Required]
        [birthdayValidator] 
        public DateTime Birthday { get; set; }







        public BookingM Booking { get; set; }

        //public string BookFlightStatus { get; set; }

       // public List<PassengerM> Passengers { get; set; }
        //public List<BookingM> Bookings { get; set; }

        public BookFlight()
        {
            NbPassengers = 1;
        }
    }
}
