using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Flight
    {
        [Key]
        public int FlightNo { get; set; }

        public virtual List<Booking> Bookings { get; }
        public virtual Pilot Pilot { get; }
        public virtual Pilot Copilot { get; }

        [Required]
        public string AirlineName { get; set; }
        [Required]
        public int CopilotId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public int FreeSeats { get; set; }
        public string Memo { get; set; }
        public Boolean NonSmokingFlight { get; set; }
        [Required]
        public int PilotId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Seat { get; set; }
        public Boolean Strikebound { get; set; }
        public string Timestamp { get; set; }
        public Boolean Utilization { get; set; }
    }
}
