using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSFly
{
    public class Flight
    {
        [Key]
        public int FlightNo { get; set; }

        public virtual ICollection<Booking> Bookings { get;  }
        
        [ForeignKey("PilotId")]
        public virtual Pilot Pilot { get;  }
        public int PilotId { get; set; }

        [ForeignKey("CopilotId")]
        public virtual Pilot Copilot { get;  }
        public int CopilotId { get; set; }

       
        public string AirlineName { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public int FreeSeats { get; set; }
        
        [Required]
        public double Price { get; set; }
        [Required]
        public int Seat { get; set; }
      
    }
}
