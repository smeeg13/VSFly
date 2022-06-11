using MVCClient.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class FlightAdminM
    {
        public int FlightNo { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public string Destination { get; set; }
        [flightDateValidator]
        public DateTime Date { get; set; }
        [Required]
        [Range(0.0,1000.0)]
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string AirlineName { get; set; }
        public int FreeSeats { get; set; }
        [Required]
        public int Seat { get; set; }
        public int PilotId { get; set; }
        public PilotAdminM Pilot { get; set; }

        public PilotAdminM Copilot { get; set; }
        public int CopilotId { get; set; }

        public double SumSales{get; set;}

        public List<Ticket> Tickets { get; set; }

    }
}
