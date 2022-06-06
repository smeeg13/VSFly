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
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string AirlineName { get; set; }
        public int FreeSeats { get; set; }
        public int Seat { get; set; }
        public int PilotId { get; set; }
        public PilotAdminM Pilot { get; set; }

        public PilotAdminM Copilot { get; set; }
        public int CopilotId { get; set; }

    }
}
