using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSFly;

namespace WebAPI.Models
{
    public class FlightAdminM
    {
        //models that are going to be displayed for everybody
                //We don't want to display all infos, only what the user needs to know

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
        public  PilotAdminM Pilot { get; set; }

        public PilotAdminM Copilot { get; set; }
        public int CopilotId { get; set; }
    }
}
