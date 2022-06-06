using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class PassengerM
    {
        public int PersonId { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
 
        public string PassportID { get;  set; }
        public string Status { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }

    }
}
